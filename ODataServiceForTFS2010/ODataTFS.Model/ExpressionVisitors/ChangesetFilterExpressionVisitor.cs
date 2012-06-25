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

    public class ChangesetFilterExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression expression;
        private FilterNode rootFilterNode;

        public ChangesetFilterExpressionVisitor(Expression expression)
        {
            this.expression = expression;
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

            var allowedProperties = new[] { "Id", "ArtifactUri", "Comment", "Committer", "CreationDate", "Owner", "Branch" };

            if (node.NodeType == ExpressionType.OrElse)
            {
                throw new NotSupportedException("Logical OR operators are not supported for Changeset Custom filters");
            }

            if (node.Left is MemberExpression && node.Right is ConstantExpression)
            {
                var fieldName = (node.Left as MemberExpression).Member.Name;
                var value = (node.Right as ConstantExpression).Value;

                if (!allowedProperties.ToList().Contains(fieldName))
                {
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "You can only filter by the following properties: {0}. (e.g. /Changesets/$filter=Committer eq 'john' and  CreationDate gt datetime'2010-10-18') ", string.Join(", ", allowedProperties)));
                }

                this.AddFilterNode(fieldName, value, FilterNode.ParseFilterExpressionType(node.NodeType), FilterNodeRelationship.And);
            }
            else if (node.Left.NodeType == ExpressionType.Conditional)
            {
                throw new NotSupportedException("Only equality and inequality operators between fields and constant expressions are allowed with Changeset Custom filters");
            }

            return base.VisitBinary(node);
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
    }
}