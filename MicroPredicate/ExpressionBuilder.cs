using System;
using System.Linq;
using System.Linq.Expressions;

namespace MicroPredicate
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (right == null)
                throw new ArgumentNullException($"The expression argument can't be null.");

            ParameterExpression param = left.Parameters.Single();

            BinaryExpression orExp = Expression.Or(left.Body, Expression.Invoke(right, param));

            return Expression.Lambda<Func<T, bool>>(orExp, param);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (right == null)
                throw new ArgumentNullException($"The expression argument can't be null.");

            ParameterExpression param = left.Parameters.Single();

            BinaryExpression andExp = Expression.And(left.Body, Expression.Invoke(right, param));

            return Expression.Lambda<Func<T, bool>>(andExp, param);
        }

        public static Expression<Func<T, bool>> Compare<T>(T constant, ComparisonType comparisonType)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "param");

            ConstantExpression constantExp = Expression.Constant(constant);

            BinaryExpression binaryExp = Expression.MakeBinary((ExpressionType)comparisonType, param, constantExp);

            return Expression.Lambda<Func<T, bool>>(binaryExp, param);
        }

        public static Expression<Func<T, bool>> False<T>() { return e => false; }

        public static Expression<Func<T, bool>> True<T>() { return e => true; }
    }

    public enum ComparisonType
    {
        Equal = 13,
        GreaterThan = 15,
        GreaterThanOrEqual = 16,
        LessThan = 20,
        LessThanOrEqual = 21,
        NotEqual = 35,
    }
}