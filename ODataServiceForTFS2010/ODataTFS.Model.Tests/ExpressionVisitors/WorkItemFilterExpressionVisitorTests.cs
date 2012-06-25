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

namespace ODataTFS.Model.Tests.ExpressionVisitors
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WorkItemFilterExpressionVisitorTests
    {
        [TestMethod]
        public void ItShouldSupportEqualityOperator()
        {
            BinaryExpression expression = Expression.Equal(Expression.Property(Expression.Constant(new WorkItem()), "Project"), Expression.Constant("myproject"));
            var visitor = new WorkItemFilterExpressionVisitor(expression);
            var node = visitor.Eval();

            Assert.AreEqual(node.Key, "Project");
            Assert.AreEqual(node.Sign, FilterExpressionType.Equal);
            Assert.AreEqual(node.Value, "myproject");
        }

        [TestMethod]
        public void ItShouldSupportInequalityOperator()
        {
            BinaryExpression expression = Expression.NotEqual(Expression.Property(Expression.Constant(new WorkItem()), "Project"), Expression.Constant("myproject"));
            var visitor = new WorkItemFilterExpressionVisitor(expression);
            var node = visitor.Eval();

            Assert.AreEqual(node.Key, "Project");
            Assert.AreEqual(node.Sign, FilterExpressionType.NotEqual);
            Assert.AreEqual(node.Value, "myproject");
        }

        [TestMethod]
        public void ItShouldSupportInequalityOperators()
        {
            var someDate = new DateTime(2011, 2, 3, 4, 5, 6); 
            var anotherDate = new DateTime(2011, 6, 7, 8, 9, 10); 

            BinaryExpression lessThanExpression = Expression.LessThan(Expression.Property(Expression.Constant(new WorkItem()), "Revision"), Expression.Constant(1));
            BinaryExpression lessThanOrEqualExpression = Expression.LessThanOrEqual(Expression.Property(Expression.Constant(new WorkItem()), "ChangedDate"), Expression.Constant(someDate));
            BinaryExpression greaterThanOrEqualExpression = Expression.GreaterThanOrEqual(Expression.Property(Expression.Constant(new WorkItem()), "Id"), Expression.Constant(3));
            BinaryExpression greaterThanExpression = Expression.GreaterThan(Expression.Property(Expression.Constant(new WorkItem()), "CreatedDate"), Expression.Constant(anotherDate));

            var visitor = new WorkItemFilterExpressionVisitor(Expression.OrElse(
                    Expression.OrElse(lessThanExpression, lessThanOrEqualExpression),
                    Expression.OrElse(greaterThanOrEqualExpression, greaterThanExpression)));

            var node = visitor.Eval();

            Assert.AreEqual("Revision", node.Key);
            Assert.AreEqual(FilterExpressionType.LessThan, node.Sign);
            Assert.AreEqual("1", node.Value);

            node = node.NextNode;
            Assert.AreEqual("ChangedDate", node.Key);
            Assert.AreEqual(FilterExpressionType.LessThanOrEqual, node.Sign);
            Assert.AreEqual(someDate.ToString(), node.Value);

            node = node.NextNode;
            Assert.AreEqual("Id", node.Key);
            Assert.AreEqual(FilterExpressionType.GreaterThanOrEqual, node.Sign);
            Assert.AreEqual("3", node.Value);

            node = node.NextNode;
            Assert.AreEqual("CreatedDate", node.Key);
            Assert.AreEqual(FilterExpressionType.GreaterThan, node.Sign);
            Assert.AreEqual(anotherDate.ToString(), node.Value);
        }

        [TestMethod]
        public void ItShouldSupportAndOperator()
        {
            var expression1 = Expression.Equal(Expression.Property(Expression.Constant(new WorkItem()), "Project"), Expression.Constant("myproject"));
            var expression2 = Expression.NotEqual(Expression.Property(Expression.Constant(new WorkItem()), "Title"), Expression.Constant("Bug in main page"));

            var visitor = new WorkItemFilterExpressionVisitor(Expression.And(expression1, expression2));

            var firstNode = visitor.Eval();
            Assert.AreEqual(firstNode.Key, "Project");
            Assert.AreEqual(firstNode.Sign, FilterExpressionType.Equal);
            Assert.AreEqual(firstNode.Value, "myproject");

            var secondNode = firstNode.NextNode;
            Assert.AreEqual(secondNode.Key, "Title");
            Assert.AreEqual(secondNode.Sign, FilterExpressionType.NotEqual);
            Assert.AreEqual(secondNode.Value, "Bug in main page");
            Assert.AreEqual(secondNode.NodeRelationship, FilterNodeRelationship.And);
        }

        [TestMethod]
        public void ItShouldSupportOrOperator()
        {
            BinaryExpression expression1 = Expression.Equal(Expression.Property(Expression.Constant(new WorkItem()), "Project"), Expression.Constant("myproject"));
            BinaryExpression expression2 = Expression.NotEqual(Expression.Property(Expression.Constant(new WorkItem()), "Title"), Expression.Constant("Bug in main page"));

            var visitor = new WorkItemFilterExpressionVisitor(Expression.OrElse(expression1, expression2));

            var firstNode = visitor.Eval();
            Assert.AreEqual(firstNode.Key, "Project");
            Assert.AreEqual(firstNode.Sign, FilterExpressionType.Equal);
            Assert.AreEqual(firstNode.Value, "myproject");

            var secondNode = firstNode.NextNode;
            Assert.AreEqual(secondNode.Key, "Title");
            Assert.AreEqual(secondNode.Sign, FilterExpressionType.NotEqual);
            Assert.AreEqual(secondNode.Value, "Bug in main page");
            Assert.AreEqual(FilterNodeRelationship.Or, secondNode.NodeRelationship);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ItShouldNotSupportMoreThanOneExpressionPerField()
        {
            BinaryExpression expression1 = Expression.Equal(Expression.Property(Expression.Constant(new WorkItem()), "Project"), Expression.Constant("myproject"));
            BinaryExpression expression2 = Expression.NotEqual(Expression.Property(Expression.Constant(new WorkItem()), "Project"), Expression.Constant("anotherproject"));

            var visitor = new WorkItemFilterExpressionVisitor(Expression.And(expression1, expression2));

            var node = visitor.Eval();
        }
    }
}