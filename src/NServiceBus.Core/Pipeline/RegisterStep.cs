namespace NServiceBus.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using NServiceBus.ObjectBuilder;
    using NServiceBus.Settings;

    /// <summary>
    /// Base class to do an advance registration of a step.
    /// </summary>
    [DebuggerDisplay("{StepId}({BehaviorType.FullName}) - {Description}")]
    public abstract class RegisterStep
    {
        readonly Func<IBuilder, IBehavior> factoryMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterStep"/> class.
        /// </summary>
        /// <param name="stepId">The unique identifier for this steps.</param>
        /// <param name="behavior">The type of <see cref="Behavior{TContext}"/> to register.</param>
        /// <param name="description">A brief description of what this step does.</param>
        /// <param name="factoryMethod">A factory method for creating the behavior.</param>
        protected RegisterStep(string stepId, Type behavior, string description, Func<IBuilder, IBehavior> factoryMethod = null)
        {
            this.factoryMethod = factoryMethod;
            BehaviorTypeChecker.ThrowIfInvalid(behavior, "behavior");
            Guard.AgainstNullAndEmpty(nameof(stepId), stepId);
            Guard.AgainstNullAndEmpty(nameof(description), description);

            BehaviorType = behavior;
            StepId = stepId;
            Description = description;
        }

        internal void ApplyContainerRegistration(ReadOnlySettings settings, IConfigureComponents container)
        {
            if (!IsEnabled(settings) || factoryMethod != null)
            {
                return;
            }

            container.ConfigureComponent(BehaviorType, DependencyLifecycle.InstancePerCall);
        }

        /// <summary>
        /// Gets the unique identifier for this step.
        /// </summary>
        public string StepId { get; }

        /// <summary>
        /// Checks if this behavior is enabled.
        /// </summary>
        public virtual bool IsEnabled(ReadOnlySettings settings)
        {
            return true;
        }

        /// <summary>
        /// Gets the description for this registration.
        /// </summary>
        public string Description { get; internal set; }

        internal IList<Dependency> Befores { get; private set; }
        internal IList<Dependency> Afters { get; private set; }

        /// <summary>
        /// Gets the type of <see cref="Behavior{TContext}"/> that is being registered.
        /// </summary>
        public Type BehaviorType { get; internal set; }

        /// <summary>
        /// Instructs the pipeline to register this step before the <paramref name="step"/> one. If the <paramref name="step"/> does not exist, this condition is ignored. 
        /// </summary>
        /// <param name="step">The <see cref="WellKnownStep"/> that we want to insert before.</param>
        public void InsertBeforeIfExists(WellKnownStep step)
        {
            Guard.AgainstNull(nameof(step), step);

            InsertBeforeIfExists((string) step);
        }

        /// <summary>
        /// Instructs the pipeline to register this step before the <paramref name="id"/> one. If the <paramref name="id"/> does not exist, this condition is ignored. 
        /// </summary>
        /// <param name="id">The unique identifier of the step that we want to insert before.</param>
        public void InsertBeforeIfExists(string id)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);

            if (Befores == null)
            {
                Befores = new List<Dependency>();
            }

            Befores.Add(new Dependency(StepId, id, Dependency.DependencyDirection.Before, false));
        }

        /// <summary>
        /// Instructs the pipeline to register this step before the <paramref name="step"/> one.
        /// </summary>
        public void InsertBefore(WellKnownStep step)
        {
            Guard.AgainstNull(nameof(step), step);

            InsertBefore((string) step);
        }

        /// <summary>
        /// Instructs the pipeline to register this step before the <paramref name="id"/> one.
        /// </summary>
        public void InsertBefore(string id)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);

            if (Befores == null)
            {
                Befores = new List<Dependency>();
            }

            Befores.Add(new Dependency(StepId, id, Dependency.DependencyDirection.Before, true));
        }

        /// <summary>
        /// Instructs the pipeline to register this step after the <paramref name="step"/> one. If the <paramref name="step"/> does not exist, this condition is ignored. 
        /// </summary>
        /// <param name="step">The unique identifier of the step that we want to insert after.</param>
        public void InsertAfterIfExists(WellKnownStep step)
        {
            Guard.AgainstNull(nameof(step), step);

            InsertAfterIfExists((string) step);
        }

        /// <summary>
        /// Instructs the pipeline to register this step after the <paramref name="id"/> one. If the <paramref name="id"/> does not exist, this condition is ignored. 
        /// </summary>
        /// <param name="id">The unique identifier of the step that we want to insert after.</param>
        public void InsertAfterIfExists(string id)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);

            if (Afters == null)
            {
                Afters = new List<Dependency>();
            }

            Afters.Add(new Dependency(StepId, id, Dependency.DependencyDirection.After, false));
        }

        /// <summary>
        /// Instructs the pipeline to register this step after the <paramref name="step"/> one.
        /// </summary>
        public void InsertAfter(WellKnownStep step)
        {
            Guard.AgainstNull(nameof(step), step);

            InsertAfter((string) step);
        }

        /// <summary>
        /// Instructs the pipeline to register this step after the <paramref name="id"/> one.
        /// </summary>
        public void InsertAfter(string id)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);

            if (Afters == null)
            {
                Afters = new List<Dependency>();
            }

            Afters.Add(new Dependency(StepId, id, Dependency.DependencyDirection.After, true));
        }

        internal BehaviorInstance CreateBehavior(IBuilder defaultBuilder)
        {
            var behavior = factoryMethod != null 
                ? factoryMethod(defaultBuilder) 
                : (IBehavior) defaultBuilder.Build(BehaviorType);

            return new BehaviorInstance(BehaviorType, behavior);
        }

        internal static RegisterStep Create(WellKnownStep wellKnownStep, Type behavior, string description, Func<IBuilder, IBehavior> factoryMethod = null)
        {
            return new DefaultRegisterStep(behavior, wellKnownStep, description, factoryMethod);
        }

        internal static RegisterStep Create(string pipelineStep, Type behavior, string description, Func<IBuilder, IBehavior> factoryMethod = null)
        {
            return new DefaultRegisterStep(behavior, pipelineStep, description, factoryMethod);
        }

        class DefaultRegisterStep : RegisterStep
        {
            public DefaultRegisterStep(Type behavior, string stepId, string description, Func<IBuilder, IBehavior> factoryMethod)
                : base(stepId, behavior, description, factoryMethod)
            {
            }
        }
    }
}