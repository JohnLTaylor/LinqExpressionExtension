using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqExpressionExtension
{
    public static class ExpressionLamdaExtension
    {
        public static Expression<Func<TParam, TResult>> AndAlso<TParam, TResult>(this Expression<Func<TParam, TResult>> funcA, Expression<Func<TParam, TResult>> funcB)
        {
            var @params = funcA.Parameters.Zip(funcB.Parameters, (a, b) => (ParamA: a, ParamB: b)).ToArray();

            return Expression.Lambda<Func<TParam, TResult>>(Expression.AndAlso(funcA.Body, Replace(funcB.Body, @params)), funcA.Parameters);
        }

        private static Expression Replace(Expression expression, (ParameterExpression ParamA, ParameterExpression ParamB)[] @params)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.Add(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.Method);
                    }

                case ExpressionType.AddChecked:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.AddChecked(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.Method);
                    }

                case ExpressionType.And:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.And(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.Method);
                    }

                case ExpressionType.AndAlso:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.AndAlso(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.Method);
                    }

                case ExpressionType.ArrayLength:
                    {
                        var exp = (UnaryExpression)expression;
                        return Expression.ArrayLength(Replace(exp.Operand, @params));
                    }

                case ExpressionType.ArrayIndex:
                    {
                        var exp = (MethodCallExpression)expression;
                        return Expression.ArrayIndex(Replace(exp.Object, @params), exp.Arguments.Select(p => Replace(p, @params)));
                    }

                case ExpressionType.Call:
                    {
                        var exp = (MethodCallExpression)expression;
                        return Expression.Call(Replace(exp.Object, @params), exp.Method, exp.Arguments.Select(p => Replace(p, @params)));
                    }

                case ExpressionType.Coalesce:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.Coalesce(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.Conversion);
                    }

                case ExpressionType.Conditional:
                    {
                        var exp = (ConditionalExpression)expression;
                        return Expression.Condition(Replace(exp.Test, @params), Replace(exp.IfTrue, @params), exp.IfFalse, exp.Type);
                    }

                case ExpressionType.Constant:
                    return expression;

                case ExpressionType.Convert:
                    {
                        var exp = (UnaryExpression)expression;
                        return Expression.Convert(Replace(exp.Operand, @params), exp.Type, exp.Method);
                    }

                case ExpressionType.GreaterThanOrEqual:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.GreaterThanOrEqual(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.IsLiftedToNull, exp.Method);
                    }

                case ExpressionType.LessThanOrEqual:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.LessThanOrEqual(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.IsLiftedToNull, exp.Method);
                    }

                case ExpressionType.MemberAccess:
                    {
                        var exp = (MemberExpression)expression;
                        return Expression.PropertyOrField(Replace(exp.Expression, @params), exp.Member.Name);
                    }

                case ExpressionType.Parameter:
                    {
                        foreach (var pair in @params)
                        {
                            if (expression == pair.ParamB)
                                return pair.ParamA;
                        }

                        return expression;
                    }

                case ExpressionType.ConvertChecked:
                    {
                        var exp = (UnaryExpression)expression;
                        return Expression.ConvertChecked(Replace(exp.Operand, @params), exp.Type, exp.Method);
                    }

                case ExpressionType.Divide:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.Divide(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.Method);
                    }

                case ExpressionType.Equal:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.Equal(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.IsLiftedToNull, exp.Method);
                    }

                case ExpressionType.ExclusiveOr:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.ExclusiveOr(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.Method);
                    }

                case ExpressionType.GreaterThan:
                    {
                        var exp = (BinaryExpression)expression;
                        return Expression.GreaterThan(Replace(exp.Left, @params), Replace(exp.Right, @params), exp.IsLiftedToNull, exp.Method);
                    }

                case ExpressionType.Invoke:
                case ExpressionType.Lambda:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.ListInit:
                case ExpressionType.MemberInit:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Negate:
                case ExpressionType.UnaryPlus:
                case ExpressionType.NegateChecked:
                case ExpressionType.New:
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                case ExpressionType.Not:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.Quote:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.TypeAs:
                case ExpressionType.TypeIs:
                case ExpressionType.Assign:
                case ExpressionType.Block:
                case ExpressionType.DebugInfo:
                case ExpressionType.Decrement:
                case ExpressionType.Dynamic:
                case ExpressionType.Default:
                case ExpressionType.Extension:
                case ExpressionType.Goto:
                case ExpressionType.Increment:
                case ExpressionType.Index:
                case ExpressionType.Label:
                case ExpressionType.RuntimeVariables:
                case ExpressionType.Loop:
                case ExpressionType.Switch:
                case ExpressionType.Throw:
                case ExpressionType.Try:
                case ExpressionType.Unbox:
                case ExpressionType.AddAssign:
                case ExpressionType.AndAssign:
                case ExpressionType.DivideAssign:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.OrAssign:
                case ExpressionType.PowerAssign:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.SubtractAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.SubtractAssignChecked:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.TypeEqual:
                case ExpressionType.OnesComplement:
                case ExpressionType.IsTrue:
                case ExpressionType.IsFalse:
                default:
                    throw new Exception($"The expression type \"{expression.NodeType}\" is not currently support.");
            }
        }
    }
}
