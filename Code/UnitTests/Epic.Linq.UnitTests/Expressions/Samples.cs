//  
//  Samples.cs
//  
//  Author:
//       Giacomo Tesio <giacomo@tesio.it>
// 
//  Copyright (c) 2010-2011 Giacomo Tesio
// 
//  This file is part of Epic.NET.
// 
//  Epic.NET is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Epic.NET is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
// 
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using NUnit.Framework;

namespace Epic.Linq.Expressions
{
    public static class Samples
    {
        public static IEnumerable<Expression> BinaryExpressions
        {
            get
            {
                yield return Expression.Add(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.AddChecked(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.Divide(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.Modulo(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.Multiply(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.MultiplyChecked(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.Power(Expression.Constant(2.0), Expression.Constant(2.0));
                yield return Expression.Subtract(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.SubtractChecked(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.And(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.Or(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.ExclusiveOr(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.LeftShift(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.RightShift(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.AndAlso(Expression.Constant(true), Expression.Constant(false));
                yield return Expression.OrElse(Expression.Constant(true), Expression.Constant(false));
                yield return Expression.Equal(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.NotEqual(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.GreaterThanOrEqual(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.GreaterThan(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.LessThan(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.LessThanOrEqual(Expression.Constant(1), Expression.Constant(2));
                yield return Expression.Coalesce(Expression.Parameter(typeof(string), "p"), Expression.Constant("test"));
                yield return Expression.ArrayIndex(Expression.Constant(new int[1]), Expression.Constant(0));
            }
        }
        
        public static IEnumerable<TestCaseData> DifferentBinaryExpressionsFromTheSameFactory
        {
            get
            {
                yield return new TestCaseData( Expression.Add(Expression.Constant(1), Expression.Constant(2)), Expression.Add(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.AddChecked(Expression.Constant(1), Expression.Constant(2)), Expression.AddChecked(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.Divide(Expression.Constant(1), Expression.Constant(2)), Expression.Divide(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.Modulo(Expression.Constant(1), Expression.Constant(2)), Expression.Modulo(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.Multiply(Expression.Constant(1), Expression.Constant(2)), Expression.Multiply(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.MultiplyChecked(Expression.Constant(1), Expression.Constant(2)), Expression.MultiplyChecked(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.Power(Expression.Constant(2.0), Expression.Constant(2.0)), Expression.Power(Expression.Constant(3.0), Expression.Constant(3.0)) );
                yield return new TestCaseData( Expression.Subtract(Expression.Constant(1), Expression.Constant(2)), Expression.Subtract(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.SubtractChecked(Expression.Constant(1), Expression.Constant(2)), Expression.SubtractChecked(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.And(Expression.Constant(1), Expression.Constant(2)), Expression.And(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.Or(Expression.Constant(1), Expression.Constant(2)), Expression.Or(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.ExclusiveOr(Expression.Constant(1), Expression.Constant(2)), Expression.ExclusiveOr(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.LeftShift(Expression.Constant(1), Expression.Constant(2)), Expression.LeftShift(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.RightShift(Expression.Constant(1), Expression.Constant(2)), Expression.RightShift(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.AndAlso(Expression.Constant(true), Expression.Constant(false)), Expression.AndAlso(Expression.Constant(false), Expression.Constant(true)) );
                yield return new TestCaseData( Expression.OrElse(Expression.Constant(true), Expression.Constant(false)), Expression.OrElse(Expression.Constant(false), Expression.Constant(true)) );
                yield return new TestCaseData( Expression.Equal(Expression.Constant(1), Expression.Constant(2)), Expression.Equal(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.NotEqual(Expression.Constant(1), Expression.Constant(2)), Expression.NotEqual(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.GreaterThanOrEqual(Expression.Constant(1), Expression.Constant(2)), Expression.GreaterThanOrEqual(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.GreaterThan(Expression.Constant(1), Expression.Constant(2)), Expression.GreaterThan(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.LessThan(Expression.Constant(1), Expression.Constant(2)), Expression.LessThan(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.LessThanOrEqual(Expression.Constant(1), Expression.Constant(2)), Expression.LessThanOrEqual(Expression.Constant(3), Expression.Constant(4)) );
                yield return new TestCaseData( Expression.Coalesce(Expression.Parameter(typeof(string), "p"), Expression.Constant("test")), Expression.Coalesce(Expression.Parameter(typeof(object), "o"), Expression.Constant("object")) );
                yield return new TestCaseData( Expression.ArrayIndex(Expression.Constant(new int[1]), Expression.Constant(0)), Expression.ArrayIndex(Expression.Constant(new int[2]), Expression.Constant(1)) );
            }
        }
        
        public static IEnumerable<TestCaseData> DifferentUnaryExpressionsFromTheSameFactory
        {
            get
            {
                yield return new TestCaseData( Expression.ArrayLength(Expression.Constant(new int[0])), Expression.ArrayLength(Expression.Constant(new int[1] {0})) ); 
                yield return new TestCaseData( Expression.Convert(Expression.Constant(1), typeof(uint)), Expression.Convert(Expression.Constant(2), typeof(uint)) ); 
                yield return new TestCaseData( Expression.ConvertChecked(Expression.Constant(1), typeof(uint)), Expression.ConvertChecked(Expression.Constant(2), typeof(uint)) );
                yield return new TestCaseData( Expression.Negate(Expression.Constant(1)), Expression.Negate(Expression.Constant(2)) );
                //yield return new TestCaseData( Expression.NegateChecked(Expression.Constant(1)), Expression.NegateChecked(Expression.Constant(2)) );
                yield return new TestCaseData( Expression.Not(Expression.Constant(true)), Expression.Not(Expression.Constant(false)) );
                Expression<Func<int, bool>> toQuote1 = i => i > 0;
                Expression<Func<int, bool>> toQuote2 = i => i < 10;
                yield return new TestCaseData( Expression.Quote(toQuote1), Expression.Quote(toQuote2) );
                yield return new TestCaseData( Expression.TypeAs(Expression.Constant(new object()), typeof(string)), Expression.TypeAs(Expression.Constant(new object()), typeof(Type)) );
                yield return new TestCaseData( Expression.UnaryPlus(Expression.Constant(1)), Expression.UnaryPlus(Expression.Constant(2)) );
            }
        }
        
        public static IEnumerable<Expression> UnaryExpressions
        {
            get
            {
                yield return Expression.ArrayLength(Expression.Constant(new int[0])); 
                yield return Expression.Convert(Expression.Constant(1), typeof(uint));
                yield return Expression.ConvertChecked(Expression.Constant(1), typeof(uint));
                yield return Expression.Negate(Expression.Constant(1));
                //yield return Expression.NegateChecked(Expression.Constant(1));
                yield return Expression.Not(Expression.Constant(true));
                Expression<Func<int, bool>> toQuote = i => i > 0;
                yield return Expression.Quote(toQuote);
                yield return Expression.TypeAs(Expression.Constant(new object()), typeof(string));
                yield return Expression.UnaryPlus(Expression.Constant(1));
            }
        }
        
        public static TypeBinaryExpression GetNewTypeBinaryExpression<T>(T value)
        {
            return Expression.TypeIs(Expression.Constant(value), typeof(int));
        }
        
        public static ListInitExpression GetNewListInitExpression()
        {
            // from http://msdn.microsoft.com/it-it/library/system.linq.expressions.listinitexpression(v=VS.90).aspx
            string tree1 = "maple";
            string tree2 = "oak";
            
            System.Reflection.MethodInfo addMethod = typeof(Dictionary<int, string>).GetMethod("Add");
            
            ElementInit elementInit1 = Expression.ElementInit(addMethod, Expression.Constant(tree1.Length), Expression.Constant(tree1));
            ElementInit elementInit2 = Expression.ElementInit(addMethod, Expression.Constant(tree2.Length), Expression.Constant(tree2));
            
            NewExpression newDictionaryExpression = Expression.New(typeof(Dictionary<int, string>));
            
            
            return Expression.ListInit(newDictionaryExpression, elementInit1, elementInit2);
        }
        

    }
}

