using System.Collections.Generic;
using System.Net;

namespace Braspag.Sdk.Contracts.Velocity
{
    public class AnalysisResponse
    {
        public HttpStatusCode HttpStatus { get; set; }

        public AnalysisResultData AnalysisResult { get; set; }

        public TransactionData Transaction { get; set; }

        public List<ErrorData> ErrorDataCollection { get; set; }

        public EmailageResultData EmailageResult { get; set; }

        public CredilinkResultData CredilinkResult { get; set; }

        public string RequestId { get; set; }
    }
}