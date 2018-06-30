using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroPredicate.Test.Expression
{
    [TestClass]
    public class ExpressionBuilderTest
    {
        private IQueryable<int> sourceArr;

        private Func<int, Expression<Func<int, bool>>> lessThan;

        private Func<int, Expression<Func<int, bool>>> greaterThan;

        private Func<int, Expression<Func<int, bool>>> equalTo;

        private Func<int, Expression<Func<int, bool>>> notEqualTo;

        private Expression<Func<int, bool>> odd;

        private Expression<Func<int, bool>> even;

        [TestInitialize]
        public void Initialize()
        {
            sourceArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 15, 19, 20 }.AsQueryable();

            lessThan = i => x => x < i;

            greaterThan = i => x => x > i;

            equalTo = i => x => x == i;

            notEqualTo = i => x => x != i;

            odd = x => x % 2 != 0;

            even = x => x % 2 == 0;

        }

        [TestMethod]
        public void WhenToCallTheOrMethodAndTheSourceExpressionIsNullThenAnArgumentNullExceptionShouldBeThrown()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionBuilder.False<int>().Or(null));

        }

        [TestMethod]
        public void WhenToCallTheAndMethodAndTheSourceExpressionIsNullThenAnArgumentNullExceptionShouldBeThrown()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionBuilder.True<int>().And(null));
        }

        [TestMethod]
        public void WhenAnExpressionTreeOrIsMountedWithAnExpressionFalseAndAnExpressionLessThanThenTheValuesLessThanTheConstantShouldBeReturned()
        {
            int constant = 10;

            Expression<Func<int, bool>> predicate = ExpressionBuilder.False<int>();

            predicate = predicate.Or(lessThan(constant));

            IQueryable<int> result = sourceArr.Where(predicate);

            foreach (var val in result)
                Assert.AreEqual(true, val < constant);
        }

        [TestMethod]
        public void WhenAnExpressionTreeAndIsMountedWithAnExpressionTrueAndAnExpressionLessThanAndExpressionEvenThenTheValuesLessThanTheConstantAndEvenShouldBeReturned()
        {
            int constant = 10;

            Expression<Func<int, bool>> predicate = ExpressionBuilder.True<int>();

            predicate = predicate.And(lessThan(constant));

            predicate = predicate.And(even);

            IQueryable<int> result = sourceArr.Where(predicate);

            foreach (var val in result)
                Assert.AreEqual(true, val < constant && val % 2 == 0);
        }

        [TestMethod]
        public void WhenAnExpressionTreeOrIsMountedWithAnExpressionFalseAndAnComparisonExpressionThenTheValuesGreaterThanTheConstantShouldBeReturned()
        {
            int constant = 15;

            Expression<Func<int, bool>> predicate = ExpressionBuilder.False<int>();

            Expression<Func<int, bool>> greaterThanExpression = ExpressionBuilder.Compare(constant, ComparisonType.GreaterThan);

            predicate = predicate.Or(greaterThanExpression);

            IQueryable<int> result = sourceArr.Where(predicate);

            foreach (var val in result)
                Assert.AreEqual(true, val > constant);
        }

        [TestMethod]
        public void WhenAnExpressionTreeAndIsMountedWithAnExpressionTrueAndAnComparisonExpressionThenTheValueEqualToConstantShouldBeReturned()
        {
            Expression<Func<int, bool>> predicate = ExpressionBuilder.True<int>();

            Expression<Func<int, bool>> greaterThanExpression = ExpressionBuilder.Compare(15, ComparisonType.Equal);

            predicate = predicate.And(greaterThanExpression);

            IQueryable<int> result = sourceArr.Where(predicate);

            Assert.AreEqual(15, result.ToList().First());
        }
    }
}
