using System;

namespace LeaveApp.Core.ViewModel
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            RequestId = Guid.NewGuid();
        }
        public Guid RequestId { get; set; }
        public object Data { get; set; }
        public object Error { get; set; }

    }
}
