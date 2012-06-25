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
    public class BuildFilterExpressionVisitorTests
    {
        [TestMethod]
        public void ItShouldSupportEqualityOperator()
        {
            BinaryExpression expression = Expression.Equal(Expression.Property(Expression.Constant(new Build()), "Project"), Expression.Constant("myproject"));
            var visitor = new BuildFilterExpressionVisitor(expression);
            var node = visitor.Eval();

            Assert.AreEqual(node.Key, "Project");
            Assert.AreEqual(node.Sign, FilterExpressionType.Equal);
            Assert.AreEqual(node.Value, "myproject");
        }

        [TestMethod]
        public void ItShouldSupportInequalityOperator()
        {
            BinaryExpression expression = Expression.NotEqual(Expression.Property(Expression.Constant(new Build()), "Project"), Expression.Constant("myproject"));
            var visitor = new BuildFilterExpressionVisitor(expression);
            var node = visitor.Eval();

            Assert.AreEqual(node.Key, "Project");
            Assert.AreEqual(node.Sign, FilterExpressionType.NotEqual);
            Assert.AreEqual(node.Value, "myproject");
        }

        [TestMethod]
        public void ItShouldSupportAndOperator()
        {
            BinaryExpression expression1 = Expression.Equal(Expression.Property(Expression.Constant(new Build()), "Project"), Expression.Constant("myproject"));
            BinaryExpression expression2 = Expression.NotEqual(Expression.Property(Expression.Constant(new Build()), "Quality"), Expression.Constant("good"));

            var visitor = new BuildFilterExpressionVisitor(Expression.And(expression1, expression2));

            var firstNode = visitor.Eval();
            Assert.AreEqual(firstNode.Key, "Project");
            Assert.AreEqual(firstNode.Sign, FilterExpressionType.Equal);
            Assert.AreEqual(firstNode.Value, "myproject");

            var secondNode = firstNode.NextNode;
            Assert.AreEqual(secondNode.Key, "Quality");
            Assert.AreEqual(secondNode.Sign, FilterExpressionType.NotEqual);
            Assert.AreEqual(secondNode.Value, "good");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ItShouldNotSupportOrOperator()
        {
            BinaryExpression expression1 = Expression.Equal(Expression.Property(Expression.Constant(new Build()), "Project"), Expression.Constant("myproject"));
            BinaryExpression expression2 = Expression.NotEqual(Expression.Property(Expression.Constant(new Build()), "Quality"), Expression.Constant("good"));

            var visitor = new BuildFilterExpressionVisitor(Expression.OrElse(expression1, expression2));

            var node = visitor.Eval();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ItShouldNotSupportMoreThanOneExpressionPerField()
        {
            BinaryExpression expression1 = Expression.Equal(Expression.Property(Expression.Constant(new Build()), "Project"), Expression.Constant("myproject"));
            BinaryExpression expression2 = Expression.NotEqual(Expression.Property(Expression.Constant(new Build()), "Project"), Expression.Constant("anotherproject"));

            var visitor = new BuildFilterExpressionVisitor(Expression.And(expression1, expression2));

            var node = visitor.Eval();
        }
    }
}
