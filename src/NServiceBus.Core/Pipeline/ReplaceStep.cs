namespace NServiceBus
{
    using System;

    class ReplaceStep
    {
        public ReplaceStep(string idToReplace, Type behavior, string description = null)
        {
            ReplaceId = idToReplace;
            Description = description;
            BehaviorType = behavior;
        }

        public string ReplaceId { get; private set; }
        public string Description { get; private set; }
        public Type BehaviorType { get; private set; }
    }
}