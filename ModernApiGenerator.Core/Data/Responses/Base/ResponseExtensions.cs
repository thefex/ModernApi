using System;
using System.Threading.Tasks;

namespace ModernApiGenerator.Core.Data.Responses.Base
{
	public static class ResponseExtensions
	{
		public static void Handle(this Response response, Action onSuccess, Action<string> onFailure)
		{
			if (response.IsSuccess)
				onSuccess();
			else
				onFailure(response.FormattedErrorMessages);
		}

		public static async Task Handle(this Response response, Func<Task> onSuccess, Action<string> onFailure)
		{
			if (response.IsSuccess)
				await onSuccess().ConfigureAwait(false);
			else
				onFailure(response.FormattedErrorMessages);
		}

		public static async Task Handle(this Response response, Func<Task> onSuccess, Func<string, Task> onFailure)
		{
			if (response.IsSuccess)
				await onSuccess().ConfigureAwait(false);
			else
				await onFailure(response.FormattedErrorMessages).ConfigureAwait(false);
		}

		public static Response CloneFailedResponse(this Response response)
		{
			var clonedResponse = new Response();

			foreach (var error in response.ErrorMessages)
				clonedResponse.AddErrorMessage(error);

			return clonedResponse;
		}

		public static Response<TResult> CloneFailedResponse<TResult>(this Response response)
		{
			if (response.IsSuccess)
				throw new InvalidOperationException("Provided response finished with success.");

			var clonedResponse = new Response<TResult>();

			foreach (var error in response.ErrorMessages)
				clonedResponse.AddErrorMessage(error);

			return clonedResponse;
		}
	}
}