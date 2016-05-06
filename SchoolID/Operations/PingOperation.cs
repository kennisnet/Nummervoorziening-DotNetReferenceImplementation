#region License
/*
Copyright 2016, Stichting Kennisnet

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System;
using System.ServiceModel;

namespace NVA_DotNetReferenceImplementation.SchoolID.Operations
{
    /// <summary>
    /// This class reflects the Ping operation of the Nummervoorziening service
    /// </summary>
    class PingOperation
    {
        private SchoolIDClient schoolIDClient;
        private PingRequest pingRequest = new PingRequest();
        private pingRequest1 pingRequestWrapper = new pingRequest1();

        /// <summary>
        /// Sets up the PingOperation object with a reference to the SchoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
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
            try
            {
                pingRequestWrapper.pingRequest = pingRequest;
                pingResponse1 pingResponseWrapper = schoolIDClient.ping(pingRequestWrapper);
                PingResponse pingReponse = pingResponseWrapper.pingResponse;
                return pingReponse.available;
            }
            catch (NullReferenceException nre)
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
        /// <returns>A string containing the version of the School ID service, or an empty string upon error</returns>
        public string GetSchoolIDVersion()
        {
            string applicationVersion = "";

            try
            {
                pingRequestWrapper.pingRequest = pingRequest;
                pingResponse1 pingResponseWrapper = schoolIDClient.ping(pingRequestWrapper);
                PingResponse pingReponse = pingResponseWrapper.pingResponse;
                applicationVersion = pingReponse.applicationVersion;
            }
            catch (NullReferenceException nre)
            {
                // Empty response; Service seems to be down
                System.Diagnostics.Debug.Write(nre.Message);
            }

            return applicationVersion;
        }
    }
}
