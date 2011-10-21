//  
//  ExpressionInspector.cs
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
using System.Reflection;
using System.Collections.ObjectModel;

namespace Epic.Linq.Expressions.Normalization
{
    /// <summary>
    /// Visit the members of System.Linq.Expressions' expressions and merge them in a new expression.
    /// </summary>
    /// <exception cref='InvalidOperationException'>
    /// Is thrown when an operation cannot be performed.
    /// </exception>
    /// <exception cref='NotSupportedException'>
    /// Is thrown when an object cannot perform an operation.
    /// </exception>
    internal sealed class ExpressionsInspector : CompositeVisitor<Expression>.VisitorBase, 
        IVisitor<Expression, UnaryExpression>, 
        IVisitor<Expression, BinaryExpression>, 
        IVisitor<Expression, ConditionalExpression>,
        IVisitor<Expression, ConstantExpression>,
        IVisitor<Expression, InvocationExpression>,
        IVisitor<Expression, LambdaExpression>,
        IVisitor<Expression, MemberExpression>,
        IVisitor<Expression, MethodCallExpression>,
        IVisitor<Expression, NewExpression>,
        IVisitor<Expression, NewArrayExpression>,
        IVisitor<Expression, MemberInitExpression>,
        IVisitor<Expression, ListInitExpression>,
        IVisitor<Expression, ParameterExpression>,
        IVisitor<Expression, TypeBinaryExpression>
    {
        public ExpressionsInspector (CompositeVisitor<Expression> composition)
            : base(composition)
        {
        }
        
        /// <summary>
        /// Adjusts the arguments for a <see cref="NewExpression"/> so that they match the given members.
        /// </summary>
        /// <param name="arguments">The arguments to adjust.</param>
        /// <param name="members">The members defining the required argument types.</param>
        /// <returns>
        /// A sequence of expressions that are equivalent to <paramref name="arguments"/>, but converted to the associated member's
        /// result type if needed.
        /// </returns>
        private static IEnumerable<Expression> AdjustArgumentsForNewExpression (IList<Expression> arguments, IList<MemberInfo> members)
        {
            for (int i = 0; i < arguments.Count; ++i) {
                Type memberReturnType = Reflection.GetMemberReturnType (members [i]);
                if (arguments [i].Type == memberReturnType)
                    yield return arguments[i];
                else
                    yield return Expression.Convert (arguments[i], memberReturnType);
            }
        }
    
        private Expression VisitExpression (Expression expression, IVisitContext state)
        {
            return VisitInner(expression, state);
        }
    
        private T VisitAndConvert<T> (T expression, string methodName, IVisitContext state) where T : Expression
        {
            if (expression == null)
                return null;
    
            T newExpression = VisitExpression (expression, state) as T;
    
            if (newExpression == null) {
                var message = string.Format (
                "When called from '{0}', expressions of type '{1}' can only be replaced with other non-null expressions of type '{2}'.",
                methodName,
                typeof(T).Name,
                typeof(T).Name);
    
                throw new InvalidOperationException (message);
            }
    
            return newExpression;
        }

        private ReadOnlyCollection<T> VisitAndConvert<T> (ReadOnlyCollection<T> expressions, string callerName, IVisitContext state) where T : Expression
        {
            return VisitList (expressions, (expression, s) => VisitAndConvert (expression, callerName, s), state);
        }

        private ReadOnlyCollection<T> VisitList<T> (ReadOnlyCollection<T> list, Func<T, IVisitContext, T> visitMethod, IVisitContext state)
            where T : class
        {
            List<T> newList = null;
    
            for (int i = 0; i < list.Count; i++) {
                T element = list [i];
                T newElement = visitMethod (element, state);
                if (newElement == null)
                    throw new NotSupportedException ("The current list only supports objects of type '" + typeof(T).Name + "' as its elements.");
    
                if (element != newElement) {
                    if (newList == null)
                        newList = new List<T> (list);
    
                    newList [i] = newElement;
                }
            }
    
            if (newList != null)
                return newList.AsReadOnly ();
            else
                return list;
        }
    
        public Expression Visit (UnaryExpression expression, IVisitContext state)
        {
            Expression newOperand = VisitExpression (expression.Operand, state);
            if (newOperand != expression.Operand) {
                if (expression.NodeType == ExpressionType.UnaryPlus)
                    return Expression.UnaryPlus (newOperand, expression.Method);
                else
                    return Expression.MakeUnary (expression.NodeType, newOperand, expression.Type, expression.Method);
            } else
                return expression;
        }

        public Expression Visit (BinaryExpression expression, IVisitContext state)
        {
            Expression newLeft = VisitExpression (expression.Left, state);
            Expression newRight = VisitExpression (expression.Right, state);
            var newConversion = (LambdaExpression)VisitExpression (expression.Conversion, state);
            if (newLeft != expression.Left || newRight != expression.Right || newConversion != expression.Conversion)
                return Expression.MakeBinary (expression.NodeType, newLeft, newRight, expression.IsLiftedToNull, expression.Method, newConversion);
            return expression;
        }
    
        public Expression Visit (TypeBinaryExpression expression, IVisitContext state)
        {
            Expression newExpression = VisitExpression (expression.Expression, state);
            if (newExpression != expression.Expression)
                return Expression.TypeIs (newExpression, expression.TypeOperand);
            return expression;
        }
    
        public Expression Visit (ConstantExpression expression, IVisitContext state)
        {
            return expression;
        }
    
        public Expression Visit (ConditionalExpression expression, IVisitContext state)
        {
            Expression newTest = VisitExpression (expression.Test, state);
            Expression newFalse = VisitExpression (expression.IfFalse, state);
            Expression newTrue = VisitExpression (expression.IfTrue, state);
            if ((newTest != expression.Test) || (newFalse != expression.IfFalse) || (newTrue != expression.IfTrue))
                return Expression.Condition (newTest, newTrue, newFalse);
            return expression;
        }
    
        public Expression Visit (ParameterExpression expression, IVisitContext state)
        {
            return expression;
        }
    
        public Expression Visit (LambdaExpression expression, IVisitContext state)
        {
            ReadOnlyCollection<ParameterExpression> newParameters = VisitAndConvert (expression.Parameters, "Visit", state);
            Expression newBody = VisitExpression (expression.Body, state);
            if ((newBody != expression.Body) || (newParameters != expression.Parameters))
                return Expression.Lambda (expression.Type, newBody, newParameters);
            return expression;

        }
    
        public Expression Visit (MethodCallExpression expression, IVisitContext state)
        {
            Expression newObject = VisitExpression (expression.Object, state);
            ReadOnlyCollection<Expression> newArguments = VisitAndConvert (expression.Arguments, "Visit", state);
            if ((newObject != expression.Object) || (newArguments != expression.Arguments))
                return Expression.Call (newObject, expression.Method, newArguments);
            return expression;

        }
    
        public Expression Visit (InvocationExpression expression, IVisitContext state)
        {
            Expression newExpression = VisitExpression (expression.Expression, state);
            ReadOnlyCollection<Expression> newArguments = VisitAndConvert (expression.Arguments, "Visit", state);
            if ((newExpression != expression.Expression) || (newArguments != expression.Arguments))
                return Expression.Invoke (newExpression, newArguments);
            return expression;
        }
    
        public Expression Visit (MemberExpression expression, IVisitContext state)
        {
            Expression newExpression = VisitExpression (expression.Expression, state);
            if (newExpression != expression.Expression)
                return Expression.MakeMemberAccess (newExpression, expression.Member);
            return expression;
        }
    
        public Expression Visit (NewExpression expression, IVisitContext state)
        {
            ReadOnlyCollection<Expression> newArguments = VisitAndConvert (expression.Arguments, "Visit", state);
            if (newArguments != expression.Arguments) {
                if (expression.Members == null)
                    return Expression.New (expression.Constructor, newArguments);
                else
                    return Expression.New (expression.Constructor, AdjustArgumentsForNewExpression (newArguments, expression.Members), expression.Members);
            }
            return expression;
        }
    
        public Expression Visit (NewArrayExpression expression, IVisitContext state)
        {
            ReadOnlyCollection<Expression> newExpressions = VisitAndConvert (expression.Expressions, "Visit", state);
            if (newExpressions != expression.Expressions) {
                var elementType = expression.Type.GetElementType ();
                if (expression.NodeType == ExpressionType.NewArrayInit)
                    return Expression.NewArrayInit (elementType, newExpressions);
                else
                    return Expression.NewArrayBounds (elementType, newExpressions);
            }
            return expression;
        }
    
        public Expression Visit (MemberInitExpression expression, IVisitContext state)
        {
            NewExpression newNewExpression = VisitExpression (expression.NewExpression, state) as NewExpression;
            if (newNewExpression == null) {
                throw new NotSupportedException (
                    "MemberInitExpressions only support non-null instances of type 'NewExpression' as their NewExpression member.");
            }
        
            ReadOnlyCollection<MemberBinding> newBindings = VisitMemberBindingList (expression.Bindings, state);
            if (newNewExpression != expression.NewExpression || newBindings != expression.Bindings)
                return Expression.MemberInit (newNewExpression, newBindings);
            return expression;
        }
    
        public Expression Visit (ListInitExpression expression, IVisitContext state)
        {
            var newNewExpression = VisitExpression (expression.NewExpression, state) as NewExpression;
            if (newNewExpression == null)
                throw new NotSupportedException ("ListInitExpressions only support non-null instances of type 'NewExpression' as their NewExpression member.");
            ReadOnlyCollection<ElementInit> newInitializers = VisitElementInitList (expression.Initializers, state);
            if (newNewExpression != expression.NewExpression || newInitializers != expression.Initializers)
                return Expression.ListInit (newNewExpression, newInitializers);
            return expression;
        }
    
        private ElementInit VisitElementInit (ElementInit elementInit, IVisitContext state)
        {
            ReadOnlyCollection<Expression> newArguments = VisitAndConvert (elementInit.Arguments, "VisitElementInit", state);
            if (newArguments != elementInit.Arguments)
                return Expression.ElementInit (elementInit.AddMethod, newArguments);
            return elementInit;
        }
    
        private MemberBinding VisitMemberBinding (MemberBinding memberBinding, IVisitContext state)
        {
            switch (memberBinding.BindingType) {
            case MemberBindingType.Assignment:
                return VisitMemberAssignment ((MemberAssignment)memberBinding, state);
            case MemberBindingType.MemberBinding:
                return VisitMemberMemberBinding ((MemberMemberBinding)memberBinding, state);
            default:
                return VisitMemberListBinding ((MemberListBinding)memberBinding, state);
            }
        }
    
        private MemberBinding VisitMemberAssignment (MemberAssignment memberAssigment, IVisitContext state)
        {
            Expression expression = VisitExpression (memberAssigment.Expression, state);
            if (expression != memberAssigment.Expression)
                return Expression.Bind (memberAssigment.Member, expression);
            return memberAssigment;
        }
    
        private MemberBinding VisitMemberMemberBinding (MemberMemberBinding binding, IVisitContext state)
        {
            ReadOnlyCollection<MemberBinding> newBindings = VisitMemberBindingList (binding.Bindings, state);
            if (newBindings != binding.Bindings)
                return Expression.MemberBind (binding.Member, newBindings);
            return binding;
        }
    
        private MemberBinding VisitMemberListBinding (MemberListBinding listBinding, IVisitContext state)
        {
            ReadOnlyCollection<ElementInit> newInitializers = VisitElementInitList (listBinding.Initializers, state);
    
            if (newInitializers != listBinding.Initializers)
                return Expression.ListBind (listBinding.Member, newInitializers);
            return listBinding;
        }
    
        private ReadOnlyCollection<MemberBinding> VisitMemberBindingList (ReadOnlyCollection<MemberBinding> expressions, IVisitContext state)
        {
            return VisitList (expressions, VisitMemberBinding, state);
        }
    
        private ReadOnlyCollection<ElementInit> VisitElementInitList (ReadOnlyCollection<ElementInit> expressions, IVisitContext state)
        {
            return VisitList (expressions, VisitElementInit, state);
        }
    }
}
