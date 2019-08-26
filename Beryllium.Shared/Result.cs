namespace Beryllium.Shared
{
   public class Result
   {
      public bool Success { get; protected set; }
      public string ErrorMessage { get; protected set; }

      public Result()
      {
         this.Success = true;
      }

      public Result(bool success, string errorMessage)
      {
         this.Success = success;
         this.ErrorMessage = errorMessage;
      }

      public Result(string errorMessage) : this(false, errorMessage)
      {
      }
   }

   public class Result<T> : Result where T : class
   {
      public T Data { get; set; }

      public Result()
      {
      }

      public Result(bool success, string errorMessage, T responseData)
      {
         this.Success = success;
         this.ErrorMessage = errorMessage;
         this.Data = responseData;
      }

      public Result(T responseData) : this(true, null, responseData)
      {
      }

      public Result(string errorMessage) : base(false, errorMessage)
      {
      }
   }
}
