﻿using System;
using System.Collections.Generic;
using Grace.Data.Immutable;
using Grace.DependencyInjection.Lifestyle;

namespace Grace.DependencyInjection.Impl.DelegateFactory
{
	/// <summary>
	/// Base class for generic func exports
	/// </summary>
	public class BaseGenericFuncExportStrategy
	{
        private readonly ExportMetadata _metadata = new ExportMetadata(null);

		/// <summary>
		/// Dispose of strategy
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		/// Initialize the strategy
		/// </summary>
		public void Initialize()
		{
		}

		/// <summary>
		/// When considering an export should it be filtered out.
		/// True by default, usually it's only false for special export types like Array ad List
		/// </summary>
		public bool AllowingFiltering
		{
			get { return false; }
		}

		/// <summary>
		/// Attributes associated with the export strategy. 
		/// Note: do not return null. Return an empty enumerable if there are none
		/// </summary>
		public IEnumerable<Attribute> Attributes
		{
            get { return ImmutableArray<Attribute>.Empty; }
		}

		/// <summary>
		/// The scope that owns this export
		/// </summary>
		public IInjectionScope OwningScope { get; set; }

		/// <summary>
		/// Export Key
		/// </summary>
		public object Key
		{
			get { return null; }
		}

		/// <summary>
		/// What environement is this strategy being exported under.
		/// </summary>
		public ExportEnvironment Environment
		{
			get { return ExportEnvironment.Any; }
		}

		/// <summary>
		/// What export priority is this being exported as
		/// </summary>
		public int Priority
		{
			get { return -1; }
		}

		/// <summary>
		/// ILifestyle associated with export
		/// </summary>
		public ILifestyle Lifestyle
		{
			get { return null; }
		}

		/// <summary>
		/// Does this export have any conditions, this is important when setting up the strategy
		/// </summary>
		public bool HasConditions
		{
			get { return false; }
		}

		/// <summary>
		/// Are the object produced by this export externally owned
		/// </summary>
		public bool ExternallyOwned
		{
			get { return false; }
		}

		/// <summary>
		/// Does this export meet the conditions to be used
		/// </summary>
		/// <param name="injectionContext"></param>
		/// <returns></returns>
		public bool MeetsCondition(IInjectionContext injectionContext)
		{
			return true;
		}

		/// <summary>
		/// An export can specify it's own strategy
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IExportStrategy> SecondaryStrategies()
		{
			return ImmutableArray<IExportStrategy>.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enrichWithDelegate"></param>
		public void EnrichWithDelegate(EnrichWithDelegate enrichWithDelegate)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<ExportStrategyDependency> DependsOn
		{
			get { return ImmutableArray<ExportStrategyDependency>.Empty; }
		}

		/// <summary>
		/// Metadata associated with this strategy
		/// </summary>
		public IExportMetadata Metadata
		{
            get { return _metadata; }
		}
	}

	/// <summary>
	/// Export strategy that creates a Func(TIn, TOut)
	/// </summary>
	/// <typeparam name="TIn"></typeparam>
	/// <typeparam name="TOut"></typeparam>
	public class GenericFuncExportStrategy<TIn, TOut> : BaseGenericFuncExportStrategy, IExportStrategy
	{
		/// <summary>
		/// Activate the export
		/// </summary>
		/// <param name="exportInjectionScope"></param>
		/// <param name="context"></param>
		/// <param name="consider"></param>
		/// <param name="locateKey"></param>
		/// <returns></returns>
		public object Activate(IInjectionScope exportInjectionScope, IInjectionContext context, ExportStrategyFilter consider, object locateKey)
		{
			IInjectionScope scope = context.RequestingScope;
			IDisposalScope disposal = context.DisposalScope;
			IInjectionTargetInfo targetInfo = context.TargetInfo;

			return new Func<TIn, TOut>(@in =>
												{
													IInjectionContext newInjectionContext = context.Clone();

													newInjectionContext.TargetInfo = targetInfo;
													newInjectionContext.RequestingScope = scope;
													newInjectionContext.DisposalScope = disposal;

													newInjectionContext.Export((s, c) => @in);

													return scope.Locate<TOut>(newInjectionContext, consider, locateKey);
												});
		}

		/// <summary>
		/// This is type that will be activated, can be used for filtering
		/// </summary>
		public Type ActivationType
		{
			get { return typeof(Func<TIn, TOut>); }
		}

		/// <summary>
		/// Usually the type.FullName, used for blacklisting purposes
		/// </summary>
		public string ActivationName
		{
			get { return typeof(Func<TIn, TOut>).FullName; }
		}

		/// <summary>
		/// Names this strategy should be known as.
		/// </summary>
		public IEnumerable<string> ExportNames
		{
			get { return ImmutableArray<string>.Empty; }
		}

		/// <summary>
		/// Type this strategy should be known as
		/// </summary>
		public IEnumerable<Type> ExportTypes
		{
			get { yield return typeof(Func<TIn, TOut>); }
		}

        /// <summary>
        /// List of keyed interface to export under
        /// </summary>
        public IEnumerable<Tuple<Type, object>> KeyedExportTypes
        {
            get { return ImmutableArray<Tuple<Type, object>>.Empty; }
        }
	}

	/// <summary>
	/// Export strategy that creates Func(TIn1, TIn2, Tout)
	/// </summary>
	/// <typeparam name="TIn1"></typeparam>
	/// <typeparam name="TIn2"></typeparam>
	/// <typeparam name="TOut"></typeparam>
	public class GenericFuncExportStrategy<TIn1, TIn2, TOut> : BaseGenericFuncExportStrategy, IExportStrategy
	{
		/// <summary>
		/// Activate the export
		/// </summary>
		/// <param name="exportInjectionScope"></param>
		/// <param name="context"></param>
		/// <param name="consider"></param>
		/// <param name="locateKey"></param>
		/// <returns></returns>
		public object Activate(IInjectionScope exportInjectionScope, IInjectionContext context, ExportStrategyFilter consider, object locateKey)
		{
			IInjectionScope scope = context.RequestingScope;
			IDisposalScope disposal = context.DisposalScope;
			IInjectionTargetInfo targetInfo = context.TargetInfo;

			return new Func<TIn1, TIn2, TOut>((in1, in2) =>
														 {
															 IInjectionContext newInjectionContext = context.Clone();

															 newInjectionContext.TargetInfo = targetInfo;
															 newInjectionContext.RequestingScope = scope;
															 newInjectionContext.DisposalScope = disposal;

															 newInjectionContext.Export((s, c) => in1);
															 newInjectionContext.Export((s, c) => in2);

															 return newInjectionContext.RequestingScope.Locate<TOut>(newInjectionContext, consider, locateKey);
														 });
		}

		/// <summary>
		/// This is type that will be activated, can be used for filtering
		/// </summary>
		public Type ActivationType
		{
			get { return typeof(Func<TIn1, TIn2, TOut>); }
		}

		/// <summary>
		/// Usually the type.FullName, used for blacklisting purposes
		/// </summary>
		public string ActivationName
		{
			get { return typeof(Func<TIn1, TIn2, TOut>).FullName; }
		}

		/// <summary>
		/// Names this strategy should be known as.
		/// </summary>
		public IEnumerable<string> ExportNames
		{
			get { return ImmutableArray<string>.Empty; }
		}

		/// <summary>
		/// Type this strategy should be known as
		/// </summary>
		public IEnumerable<Type> ExportTypes
		{
			get { yield return typeof(Func<TIn1, TIn2, TOut>); }
		}

        /// <summary>
        /// List of keyed interface to export under
        /// </summary>
        public IEnumerable<Tuple<Type, object>> KeyedExportTypes
        {
            get { return ImmutableArray<Tuple<Type, object>>.Empty; }
        }
	}

	/// <summary>
	/// Export strategy that creates a Func(TIn1, Tin2, TIn3, TOut)
	/// </summary>
	/// <typeparam name="TIn1"></typeparam>
	/// <typeparam name="TIn2"></typeparam>
	/// <typeparam name="TIn3"></typeparam>
	/// <typeparam name="TOut"></typeparam>
	public class GenericFuncExportStrategy<TIn1, TIn2, TIn3, TOut> : BaseGenericFuncExportStrategy, IExportStrategy
	{
		/// <summary>
		/// Activate the export
		/// </summary>
		/// <param name="exportInjectionScope"></param>
		/// <param name="context"></param>
		/// <param name="consider"></param>
		/// <param name="locateKey"></param>
		/// <returns></returns>
		public object Activate(IInjectionScope exportInjectionScope, IInjectionContext context, ExportStrategyFilter consider, object locateKey)
		{
			IInjectionScope scope = context.RequestingScope;
			IDisposalScope disposal = context.DisposalScope;
			IInjectionTargetInfo targetInfo = context.TargetInfo;

			return new Func<TIn1, TIn2, TIn3, TOut>((in1, in2, in3) =>
																 {
																	 IInjectionContext newInjectionContext = context.Clone();

																	 newInjectionContext.TargetInfo = targetInfo;
																	 newInjectionContext.RequestingScope = scope;
																	 newInjectionContext.DisposalScope = disposal;

																	 newInjectionContext.Export((s, c) => in1);
																	 newInjectionContext.Export((s, c) => in2);
																	 newInjectionContext.Export((s, c) => in3);

																	 return newInjectionContext.RequestingScope.Locate<TOut>(newInjectionContext, consider, locateKey);
																 });
		}

		/// <summary>
		/// This is type that will be activated, can be used for filtering
		/// </summary>
		public Type ActivationType
		{
			get { return typeof(Func<TIn1, TIn2, TIn3, TOut>); }
		}

		/// <summary>
		/// Usually the type.FullName, used for blacklisting purposes
		/// </summary>
		public string ActivationName
		{
			get { return typeof(Func<TIn1, TIn2, TIn3, TOut>).FullName; }
		}

		/// <summary>
		/// Names this strategy should be known as.
		/// </summary>
		public IEnumerable<string> ExportNames
		{
			get { return ImmutableArray<string>.Empty; }
		}

		/// <summary>
		/// Type this strategy should be known as
		/// </summary>
		public IEnumerable<Type> ExportTypes
		{
			get { yield return typeof(Func<TIn1, TIn2, TIn3, TOut>); }
		}

        /// <summary>
        /// List of keyed interface to export under
        /// </summary>
        public IEnumerable<Tuple<Type, object>> KeyedExportTypes
        {
            get { return ImmutableArray<Tuple<Type, object>>.Empty; }
        }
	}

	/// <summary>
	/// Creates a new Func(TIn1, TIn2, TIn3, TIn4, TOut)
	/// </summary>
	/// <typeparam name="TIn1"></typeparam>
	/// <typeparam name="TIn2"></typeparam>
	/// <typeparam name="TIn3"></typeparam>
	/// <typeparam name="TIn4"></typeparam>
	/// <typeparam name="TOut"></typeparam>
	public class GenericFuncExportStrategy<TIn1, TIn2, TIn3, TIn4, TOut> : BaseGenericFuncExportStrategy, IExportStrategy
	{
		/// <summary>
		/// Activate the export
		/// </summary>
		/// <param name="exportInjectionScope"></param>
		/// <param name="context"></param>
		/// <param name="consider"></param>
		/// <param name="locateKey"></param>
		/// <returns></returns>
		public object Activate(IInjectionScope exportInjectionScope, IInjectionContext context, ExportStrategyFilter consider, object locateKey)
		{
			IInjectionScope scope = context.RequestingScope;
			IDisposalScope disposal = context.DisposalScope;
			IInjectionTargetInfo targetInfo = context.TargetInfo;

			return new Func<TIn1, TIn2, TIn3, TIn4, TOut>((in1, in2, in3, in4) =>
																		 {
																			 IInjectionContext newInjectionContext = context.Clone();

																			 newInjectionContext.TargetInfo = targetInfo;
																			 newInjectionContext.RequestingScope = scope;
																			 newInjectionContext.DisposalScope = disposal;

																			 newInjectionContext.Export((s, c) => in1);
																			 newInjectionContext.Export((s, c) => in2);
																			 newInjectionContext.Export((s, c) => in3);
																			 newInjectionContext.Export((s, c) => in4);

																			 return
																				 newInjectionContext.RequestingScope.Locate<TOut>(newInjectionContext, consider, locateKey);
																		 });
		}

		/// <summary>
		/// This is type that will be activated, can be used for filtering
		/// </summary>
		public Type ActivationType
		{
			get { return typeof(Func<TIn1, TIn2, TIn3, TIn4, TOut>); }
		}

		/// <summary>
		/// Usually the type.FullName, used for blacklisting purposes
		/// </summary>
		public string ActivationName
		{
			get { return typeof(Func<TIn1, TIn2, TIn3, TIn4, TOut>).FullName; }
		}

		/// <summary>
		/// Names this strategy should be known as.
		/// </summary>
		public IEnumerable<string> ExportNames
		{
			get { return ImmutableArray<string>.Empty; }
		}

		/// <summary>
		/// Type this strategy should be known as
		/// </summary>
		public IEnumerable<Type> ExportTypes
		{
			get { yield return typeof(Func<TIn1, TIn2, TIn3, TIn4, TOut>); }
		}

        /// <summary>
        /// List of keyed interface to export under
        /// </summary>
        public IEnumerable<Tuple<Type, object>> KeyedExportTypes
        {
            get { return ImmutableArray<Tuple<Type, object>>.Empty; }
        }
	}

	/// <summary>
	/// Creates a new Func(TIn1, TIn2, TIn3, TIn4, TIn5, TOut)
	/// </summary>
	/// <typeparam name="TIn1"></typeparam>
	/// <typeparam name="TIn2"></typeparam>
	/// <typeparam name="TIn3"></typeparam>
	/// <typeparam name="TIn4"></typeparam>
	/// <typeparam name="TIn5"></typeparam>
	/// <typeparam name="TOut"></typeparam>
	public class GenericFuncExportStrategy<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> : BaseGenericFuncExportStrategy,
		IExportStrategy
	{
		/// <summary>
		/// Activate the export
		/// </summary>
		/// <param name="exportInjectionScope"></param>
		/// <param name="context"></param>
		/// <param name="consider"></param>
		/// <param name="locateKey"></param>
		/// <returns></returns>
		public object Activate(IInjectionScope exportInjectionScope, IInjectionContext context, ExportStrategyFilter consider, object locateKey)
		{
			IInjectionScope scope = context.RequestingScope;
			IDisposalScope disposal = context.DisposalScope;
			IInjectionTargetInfo targetInfo = context.TargetInfo;

			return new Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(
				(in1, in2, in3, in4, in5) =>
				{
					IInjectionContext newInjectionContext = context.Clone();

					newInjectionContext.TargetInfo = targetInfo;
					newInjectionContext.RequestingScope = scope;
					newInjectionContext.DisposalScope = disposal;

					newInjectionContext.Export((s, c) => in1);
					newInjectionContext.Export((s, c) => in2);
					newInjectionContext.Export((s, c) => in3);
					newInjectionContext.Export((s, c) => in4);
					newInjectionContext.Export((s, c) => in5);

					return
						newInjectionContext.RequestingScope.Locate<TOut>(newInjectionContext, consider, locateKey);
				});
		}

		/// <summary>
		/// This is type that will be activated, can be used for filtering
		/// </summary>
		public Type ActivationType
		{
			get { return typeof(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>); }
		}

		/// <summary>
		/// Usually the type.FullName, used for blacklisting purposes
		/// </summary>
		public string ActivationName
		{
			get { return typeof(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>).FullName; }
		}

		/// <summary>
		/// Names this strategy should be known as.
		/// </summary>
		public IEnumerable<string> ExportNames
		{
			get { return ImmutableArray<string>.Empty; }
		}

		/// <summary>
		/// Type this strategy should be known as
		/// </summary>
		public IEnumerable<Type> ExportTypes
		{
			get { yield return typeof(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>); }
		}

        /// <summary>
        /// List of keyed interface to export under
        /// </summary>
        public IEnumerable<Tuple<Type, object>> KeyedExportTypes
        {
            get { return ImmutableArray<Tuple<Type, object>>.Empty; }
        }
	}
}