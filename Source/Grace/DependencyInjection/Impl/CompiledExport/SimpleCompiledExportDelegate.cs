﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Grace.DependencyInjection.Impl.CompiledExport
{
	public class SimpleCompiledExportDelegate : InstanceCompiledExportDelegate
	{
		public SimpleCompiledExportDelegate(CompiledExportDelegateInfo exportDelegateInfo, IExportStrategy exportStrategy) :
			base(exportDelegateInfo, exportStrategy,null)
		{
		}

		/// <summary>
		/// This method generates the compiled delegate
		/// </summary>
		/// <returns></returns>
		protected override ExportActivationDelegate GenerateDelegate()
		{
			SetUpParameterExpressions();

			SetUpInstanceVariableExpression();

			CreateInstantiationExpression();

			CreateCustomInitializeExpressions();

			bodyExpressions.Add(Expression.Call(injectionContextParameter, PopCurrentInjectionInfo));

			// only add the return expression if there was no enrichment
			CreateReturnExpression();

			MethodInfo closedInfo = PushCurrentInjectionInfo.MakeGenericMethod(exportDelegateInfo.ActivationType);

			List<Expression> methodExpressions = new List<Expression>
			                                     {
				                                     Expression.Call(injectionContextParameter, 
																						closedInfo,
																						Expression.Constant(owningStrategy))
			                                     };

			methodExpressions.AddRange(GetImportExpressions());
			methodExpressions.AddRange(instanceExpressions);
			methodExpressions.AddRange(bodyExpressions);

			BlockExpression body = Expression.Block(localVariables, methodExpressions);

			return Expression.Lambda<ExportActivationDelegate>(body,
				exportStrategyScopeParameter,
				injectionContextParameter).Compile();
		}
	}
}