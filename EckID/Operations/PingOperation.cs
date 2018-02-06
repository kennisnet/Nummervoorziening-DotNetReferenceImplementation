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

namespace EckID.Operations
{
    using System;

    /// <summary>
    /// This class reflects the Ping operation of the Nummervoorziening service
    /// </summary>
    public class PingOperation
    {
        /// <summary>
        /// The EckID object for communication with the service
        /// </summary>
        private readonly EckIDPortClient _eckIdClient;

        /// <summary>
        /// The actual Ping Request object
        /// </summary>
        private readonly PingRequest _pingRequest = new PingRequest();

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly pingRequest1 _pingRequestWrapper = new pingRequest1();

        /// <summary>
        /// Initializes a new instance of the <see cref="PingOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="eckIdClient">An initialized EckIDPortClient proxy class</param>
        public PingOperation(EckIDPortClient eckIdClient)
        {
            _eckIdClient = eckIdClient;
        }

        /// <summary>
        /// Checks through a PingRequest whether the EckID servivce is available
        /// </summary>
        /// <returns>TRUE if the service is up and running</returns>
        public bool IsAvailable()
        {
            try
            {
                _pingRequestWrapper.pingRequest = _pingRequest;
                pingResponse1 pingResponseWrapper = _eckIdClient.ping(_pingRequestWrapper);
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
        public DateTime? GetEckIDDateTime()
        {           
            try
            {
                _pingRequestWrapper.pingRequest = _pingRequest;
                pingResponse1 pingResponseWrapper = _eckIdClient.ping(_pingRequestWrapper);
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
        public string GetEckIDVersion()
        {
            string applicationVersion = string.Empty;

            try
            {
                _pingRequestWrapper.pingRequest = _pingRequest;
                pingResponse1 pingResponseWrapper = _eckIdClient.ping(_pingRequestWrapper);
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
