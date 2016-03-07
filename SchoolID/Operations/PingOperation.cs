using System;

namespace NVA_DotNetReferenceImplementation.SchoolID.Operations
{
    class PingOperation
    {
        private SchoolIDClient schoolIDClient;
        private PingRequest pingRequest = new PingRequest();
        private pingRequest1 pingRequestWrapper = new pingRequest1();

        public PingOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
        }

        /// <summary>
        /// Checks through a PingRequest whether the SchoolID servivce is available
        /// </summary>
        /// <returns>TRUE if the service is up and running</returns>
        public bool IsAvailable()
        {
            try {
                pingRequestWrapper.pingRequest = pingRequest;
                pingResponse1 pingResponseWrapper = schoolIDClient.ping(pingRequestWrapper);
                PingResponse pingReponse = pingResponseWrapper.pingResponse;
                return pingReponse.available;
            } catch (NullReferenceException nre)
            {
                // Empty response; Service seems to be down
                System.Diagnostics.Debug.Write(nre.Message);
            }

            return false;
        }

        /// <summary>
        /// Retrieves the current DateTime of the School ID server
        /// </summary>
        /// <returns>Null upon errors, else the DateTime retrieved from the server</returns>
        public DateTime? GetSchoolIDDateTime()
        {           
            try
            {
                pingRequestWrapper.pingRequest = pingRequest;
                pingResponse1 pingResponseWrapper = schoolIDClient.ping(pingRequestWrapper);
                PingResponse pingReponse = pingResponseWrapper.pingResponse;
                return pingReponse.systemTime;
            }
            catch (NullReferenceException nre)
            {
                // Empty response; Service seems to be down
                System.Diagnostics.Debug.Write(nre.Message);
            }

            return null;
        }

        /// <summary>
        /// Retrieves the current application version of the School ID service.
        /// </summary>
        /// <returns>A string containing the version of the School ID service</returns>
        public string GetSchoolIDVersion()
        {
            try
            {
                pingRequestWrapper.pingRequest = pingRequest;
                pingResponse1 pingResponseWrapper = schoolIDClient.ping(pingRequestWrapper);
                PingResponse pingReponse = pingResponseWrapper.pingResponse;
                return pingReponse.applicationVersion;
            }
            catch (NullReferenceException nre)
            {
                // Empty response; Service seems to be down
                System.Diagnostics.Debug.Write(nre.Message);
            }

            return "";
        }
    }
}
