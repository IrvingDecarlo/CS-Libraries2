using System;

namespace Cephei.Valuables
{
    /// <summary>
    /// The Gauge is the object implementation of IGauge, offering its functionalities. It uses an IGauge[double].
    /// </summary>
    public class Gauge : IGauge<double>
    {
        /// <summary>
        /// Instantiates a new Gauge, setting its values.
        /// </summary>
        /// <param name="value">Value (or precentage) to set.</param>
        /// <param name="maxvalue">Max value.</param>
        /// <param name="pct">Should it set the 'value' param to its value or to its percentage?</param>
        /// <param name="freeflow">Should it be freeflow?</param>
        public Gauge(double value, double maxvalue, bool pct=false, bool freeflow=false)
        {
            max_value = maxvalue;
            this.freeflow = freeflow;
            if (pct) Percentage = value;
            else Value = value;
        }

        //
        // OVERRIDES
        //

        /// <summary>
        /// Sets/gets MaxValue, adjusting its current value based on the percentage.
        /// May throw exception if this gauge is not freeflow and MaxValue is set to the negatives or to 0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual double MaxValue
        {
            set
            {
                if (!Freeflow && value <= 0) throw new ArgumentOutOfRangeException("MaxValue", "MaxValue cannot be negative (" + value.ToString() + ").");
                max_value = value;
                this.value = percent * value;
            }
            get => max_value;
        }

        /// <summary>
        /// Sets/gets the gauge's percentage, adjusting its value based off of its max value. May throw exceptions if this gauge cannot overflow.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual double Percentage
        {
            set
            {
                if (!Freeflow)
                {
                    if (value > 1) throw new ArgumentOutOfRangeException("Percentage", "Percentage cannot be higher than 100% (" + value.ToString("P") + ").");
                    else if (value < 0) throw new ArgumentOutOfRangeException("Percentage", "Percentage cannot be lower than 0% (" + value.ToString("P") + ").");
                }
                percent = value;
                this.value = value * max_value;
            }
            get => percent;
        }

        /// <summary>
        /// The gauge's current value. May throw exceptions if not Freeflow and it is set to be greater than MaxValue or lower than 0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual double Value
        {
            set
            {
                if (!Freeflow)
                {
                    if (value > max_value)
                        throw new ArgumentOutOfRangeException("Value", "Value cannot be higher than MaxValue (" + value.ToString() + " / " + MaxValue.ToString() + ").");
                    else if (value < 0) throw new ArgumentOutOfRangeException("Value", "Value cannot be lower than 0 (" + value.ToString() + ").");
                }
                this.value = value;
                percent = value / MaxValue;
            }
            get => value;
        }

        /// <summary>
        /// Returns a string with the gauge's values.
        /// </summary>
        /// <returns>A string with the gauge's values.</returns>
        public override string ToString() => this.ToGaugeString("P");

        //
        // PUBLIC
        //

        // METHODS

        /// <summary>
        /// Sets the MaxValue without adjusting current value to maintain the percentage. If Freeflow is false then MaxValue cannot be below 0 nor lower than current value.
        /// </summary>
        /// <param name="value">The value to set MaxValue to.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual void SetMaxValue(double value)
        {
            if (!Freeflow)
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", "MaxValue cannot be negative (" + value.ToString() + ").");
                else if (value < Value)
                    throw new ArgumentOutOfRangeException("value", "MaxValue cannot be lower than current value (" + value.ToString() + "/" + Value.ToString() + ").");
            }
            max_value = value;
            percent = this.value / value;
        }

        // PROPERTIES

        /// <summary>
        /// Can this gauge's values flow out of bounds (have percentages that exceed 100%, negative MaxValues, etc).
        /// Will throw an exception if the gauge has a percentage out of the 0~100% range.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual bool Freeflow
        {
            set
            {
                if (value.Equals(false) && (Percentage > 1 || Percentage < 0))
                    throw new InvalidOperationException("Freeflow cannot be set to false since the percentage is out of the freeflow range (" + Percentage.ToString("P") + ").");
                else freeflow = value;
            }
            get => freeflow;
        }

        //
        // PRIVATE
        //

        // VARIABLES

        private bool freeflow;
        private double value, max_value, percent;
    }
}