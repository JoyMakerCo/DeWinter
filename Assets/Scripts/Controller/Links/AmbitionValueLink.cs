using System;
using UFlow;

namespace Ambition
{
    public class AmbitionValueLink<T> : ULink<T>
    {
        public string ValueID=null;
        public T Value;
        public bool ValidateOnInit = false;
        public bool ValidateOnCallback = false;
        public override void SetValue(T value) { Value = value; }

        override sealed public void Initialize()
        {
            if (ValidateOnCallback)
            {
                if (string.IsNullOrEmpty(ValueID))
                    AmbitionApp.Subscribe<T>(HandleValue);
                else
                    AmbitionApp.Subscribe<T>(ValueID, HandleValue);
            }
        }

        override sealed public bool Validate()
        {
            return ValidateOnInit && Validate(GetValue());
// TODO: Implment Value Service and then get rid of GetValue()
            // if (!ValidateOnInit) return false;
            // T value = (string.IsNullOrEmpty(ValueID))
            //     ? AmbitionApp.GetValue<T>(ValueID)
            //     : AmbitionApp.GetValue<T>();
            // return Validate(value);
        }

		override sealed public void Dispose()
        {
            AmbitionApp.Unsubscribe<T>(ValueID, HandleValue);
            AmbitionApp.Unsubscribe<T>(HandleValue);
        }

        private void HandleValue(T value)
        {
            if (Validate(value)) Activate();
        }

        protected virtual T GetValue() { return default(T); }

        protected virtual bool Validate(T value) { return value.Equals(Value); }
    }
}
