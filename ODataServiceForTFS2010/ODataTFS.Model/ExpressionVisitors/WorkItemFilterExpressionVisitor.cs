// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;

    public class WorkItemFilterExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression expression;
        private FilterNode rootFilterNode;
        private IDictionary<BinaryExpression, ExpressionType> expressionsMap;
        private ISet<Expression> ignorableExpressions;

        public WorkItemFilterExpressionVisitor(Expression expression)
        {
            this.expression = expression;
            this.expressionsMap = new Dictionary<BinaryExpression, ExpressionType>();
            this.ignorableExpressions = new HashSet<Expression>();
        }

        public FilterNode Eval()
        {
            if (this.expression == null)
            {
                return this.rootFilterNode;
            }

            this.Visit(this.expression);

            return this.rootFilterNode;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (!this.ignorableExpressions.Contains(node))
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                        this.AddEqualityFilterNode(node);
                        break;
                    case ExpressionType.GreaterThan:
                    case ExpressionType.LessThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.LessThanOrEqual:
                        this.AddInequalityFilterNode(node);
                        break;
                    case ExpressionType.OrElse:
                    case ExpressionType.AndAlso:
                        var field = node.Right as BinaryExpression;
                        if (field != null)
                        {
                            this.expressionsMap.Add(field, node.NodeType);
                        }

                        break;
                }
            }

            return base.VisitBinary(node);
        }

        private static string[] GetAllowedWorkItemProperties()
        {
            return new[]
            {
                "Id", "AreaPath", "IterationPath", "Revision", "Priority", "Severity", "StackRank", "Project", "AssignedTo", 
                "CreatedDate", "CreatedBy", "ChangedDate", "ChangedBy", "ResolvedBy", "Title", "State", "Type", "Reason", 
                "Description", "ReproSteps", "FoundInBuild", "IntegratedInBuild"
            };
        }

        private void AddInequalityFilterNode(BinaryExpression node)
        {
            var field = default(MemberExpression);
            var value = default(ConstantExpression);

            if (node.Left is MethodCallExpression)
            {
                field = (node.Left as MethodCallExpression).Arguments[0] as MemberExpression;
                value = (node.Left as MethodCallExpression).Arguments[1] as ConstantExpression;
            }
            else if (node.Left is MemberExpression && node.Right is ConstantExpression)
            {
                field = node.Left as MemberExpression;
                value = node.Right as ConstantExpression;
            }

            if (field != null && value != null)
            {
                if (!GetAllowedWorkItemProperties().ToList().Contains(field.Member.Name))
                {
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "You can only filter by the following properties: {0}. (e.g. /WorkItems/$filter=AssignedTo eq 'john' and  CreatedDate gt datetime'2010-10-18') ", string.Join(", ", GetAllowedWorkItemProperties())));
                }

                this.AddFilterNode(field.Member.Name, value.Value, FilterNode.ParseFilterExpressionType(node.NodeType), this.GetLogicalOperator(node));
            }
        }

        private void AddEqualityFilterNode(BinaryExpression node)
        {
            if (node.Left is MemberExpression && node.Right is ConstantExpression)
            {
                var field = node.Left as MemberExpression;
                var value = node.Right as ConstantExpression;

                if (!GetAllowedWorkItemProperties().ToList().Contains(field.Member.Name))
                {
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "You can only filter by the following properties: {0}. (e.g. /WorkItems/$filter=AssignedTo eq 'john' and  CreatedDate gt datetime'2010-10-18') ", string.Join(", ", GetAllowedWorkItemProperties())));
                }

                this.AddFilterNode(field.Member.Name, value.Value, FilterNode.ParseFilterExpressionType(node.NodeType), this.GetLogicalOperator(node));
            }
            else if (node.Left is ConditionalExpression && (node.Right is UnaryExpression || node.Right is ConstantExpression))
            {
                var leftExpression = node.Left as ConditionalExpression;

                var rightExpressionValue = true;
                var isRightExpressionConstantExpression = true;
                if (node.Right is UnaryExpression)
                {
                    var rightExpression = node.Right as UnaryExpression;
                    isRightExpressionConstantExpression = rightExpression.Operand is ConstantExpression;
                    rightExpressionValue = (bool)(rightExpression.Operand as ConstantExpression).Value;
                }

                if (leftExpression.IfFalse is UnaryExpression && isRightExpressionConstantExpression)
                {
                    var evaluateToTrue = rightExpressionValue ^ node.NodeType == ExpressionType.NotEqual;

                    var falsePredicate = leftExpression.IfFalse as UnaryExpression;
                    if (falsePredicate.Operand is MethodCallExpression)
                    {
                        var condition = falsePredicate.Operand as MethodCallExpression;
                        if (condition.Method.Name.Equals("Contains", StringComparison.OrdinalIgnoreCase))
                        {
                            if (condition.Object is MemberExpression && condition.Arguments[0] is ConstantExpression)
                            {
                                this.ignorableExpressions.Add(leftExpression.Test);

                                var fieldName = (condition.Object as MemberExpression).Member.Name;
                                var value = (condition.Arguments[0] as ConstantExpression).Value;

                                this.AddFilterNode(fieldName, value, evaluateToTrue ? FilterExpressionType.Contains : FilterExpressionType.NotContains, this.GetLogicalOperator(node));
                            }
                        }
                    }
                }
            }
        }

        private void AddFilterNode(string fieldName, object value, FilterExpressionType expressionType, FilterNodeRelationship filterNodeRelationship)
        {
            if (this.rootFilterNode != null && this.rootFilterNode.SingleOrDefault(p => p.Key.Equals(fieldName, StringComparison.OrdinalIgnoreCase)) != null)
            {
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "More than one filter expression found for attribute {0}. Only one filter expression per attribute is supported.", fieldName));
            }

            var filterNode = new FilterNode { Key = fieldName, Value = (value == null) ? "null" : value.ToString(), Sign = expressionType, NodeRelationship = filterNodeRelationship };
            if (this.rootFilterNode != null)
            {
                this.rootFilterNode.AddNode(filterNode);
            }
            else
            {
                this.rootFilterNode = filterNode;
            }
        }

        private FilterNodeRelationship GetLogicalOperator(BinaryExpression node)
        {
            var value = ExpressionType.AndAlso;
            this.expressionsMap.TryGetValue(node, out value);

            if (value == ExpressionType.OrElse)
            {
                return FilterNodeRelationship.Or;
            }

            return FilterNodeRelationship.And;
        }
    }
}
