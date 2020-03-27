using System;
using System.Collections.Generic;
using System.Text;

namespace OMI.Core.Entity
{
  [Serializable]
  public class ValidationError
  {
		/// <summary>
		/// Creates a new validation error.
		/// </summary>
		public ValidationError(string propertyName, string errorMessage) : this(propertyName, errorMessage, null)
		{
		}

		/// <summary>
		/// Creates a new ValidationError.
		/// </summary>
		public ValidationError(string propertyName, string errorMessage, object attemptedValue)
		{
			PropertyName = propertyName;
			ErrorMessage = errorMessage;
			AttemptedValue = attemptedValue;
		}

		/// <summary>
		/// The name of the property.
		/// </summary>
		public string PropertyName { get; set; }

		/// <summary>
		/// The error message
		/// </summary>
		public string ErrorMessage { get; set; }

		/// <summary>
		/// The property value that caused the failure.
		/// </summary>
		public object AttemptedValue { get; set; }

		/// <summary>
		/// Custom state associated with the failure.
		/// </summary>
		public object CustomState { get; set; }

		/// <summary>
		/// Gets or sets the error code.
		/// </summary>
		public string ErrorCode { get; set; }

		/// <summary>
		/// Gets or sets the formatted message arguments.
		/// These are values for custom formatted message in validator resource files
		/// Same formatted message can be reused in UI and with same number of format placeholders
		/// Like "Value {0} that you entered should be {1}"
		/// </summary>
		public object[] FormattedMessageArguments { get; set; }

		/// <summary>
		/// Gets or sets the formatted message placeholder values.
		/// </summary>
		public Dictionary<string, object> FormattedMessagePlaceholderValues { get; set; }

		/// <summary>
		/// Creates a textual representation of the failure.
		/// </summary>
		public override string ToString()
		{
			return ErrorMessage;
		}
	}
}
