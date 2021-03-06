﻿#region License
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

using System.Collections.Generic;

namespace EckID
{
    /// <summary>
    /// Entity with information about generated in batch operation Eck IDs.
    /// </summary>
    public class EckIDBatch
    {
        private Dictionary<int, string> _successList;

        private Dictionary<int, string> _failureList;

        /// <summary>
        /// Standard constructor
        /// </summary>
        public EckIDBatch()
        {

        }

        /// <summary>
        /// Constructor for creating a EckIDBatch with Dictionary formatted lists
        /// </summary>
        /// <param name="successList">List of succesful generated EckIds</param>
        /// <param name="failedList">List of indices for which generation of EckId failed</param>
        public EckIDBatch(Dictionary<int, string> successList, Dictionary<int, string> failedList)
        {
            SetSuccessList(successList);
            SetFailedList(failedList);
        }

        /// <summary>
        /// Sets the Dictionary with indexes of passed hashed PGN as keys and School IDs as values
        /// </summary>
        /// <param name="successList">Dictionary with indexes of passed hashed PGN as keys and School IDs as values</param>
        public void SetSuccessList(Dictionary<int, string> successList)
        {
            _successList = successList;
        }

        /// <summary>
        /// Sets the Dictionary with indexes of passed hashed PGN as keys and School IDs as values
        /// </summary>
        /// <param name="successList">ListedEckIdSuccess array with indexes of passed hashed PGN as keys and School IDs as values</param>
        public void SetSuccessList(ListedEntitySuccess[] successList)
        {
            SetSuccessList(listedEckIdSuccessToDictionary(successList));
        }


        /// <summary>
        /// Sets the Dictionary with indexes of passed hashed PGN as keys and error messages as values
        /// </summary>
        /// <param name="failedList">Dictionary with indexes of passed hashed PGN as keys and error messages as values</param>
        public void SetFailedList(Dictionary<int, string> failedList)
        {
            _failureList = failedList;
        }

        /// <summary>
        /// Sets the Dictionary with indexes of passed hashed PGN as keys and error messages as values
        /// </summary>
        /// <param name="failureList">ListedEckIdFailure array with indexes of passed hashed PGN as keys and error messages as values</param>
        public void SetFailedList(ListedEntityFailure[] failureList)
        {
            SetFailedList(listedEckIdFailureToDictionary(failureList));
        }


        /// <summary>
        /// Gets the Dictionary with indexes of passed hashed PGN as keys and School IDs as values
        /// </summary>
        /// <returns>Dictionary with indexes of passed hashed PGN as keys and School IDs as values</returns>
        public Dictionary<int, string> GetSuccessList()
        {
            return _successList;
        }

        /// <summary>
        /// Gets the Dictionary with indexes of passed hashed PGN as keys and error messages as values
        /// </summary>
        /// <returns>Dictionary with indexes of passed hashed PGN as keys and error messages as values</returns>
        public Dictionary<int, string> GetFailedList()
        {
            return _failureList;
        }
        
        /// <summary>
        /// Helper function to translate ListedEckIdSuccess array to Dictionary
        /// </summary>
        /// <param name="successList">Array of ListedEckIdSuccess</param>
        /// <returns>Dictionary</returns>
        private Dictionary<int, string> listedEckIdSuccessToDictionary(ListedEntitySuccess[] successList)
        {
            Dictionary<int, string> successDictionary = new Dictionary<int, string>();
            if (successList != null)
            {
                foreach (ListedEntitySuccess successEntry in successList)
                {
                    successDictionary.Add(successEntry.index, successEntry.value);
                }
            }

            return successDictionary;
        }

        /// <summary>
        /// Helper function to translate ListedEckIdFailure to Dictionary
        /// </summary>
        /// <param name="failureList">Array of ListedEckIdFailure</param>
        /// <returns>Dictionary</returns>
        private Dictionary<int, string> listedEckIdFailureToDictionary(ListedEntityFailure[] failureList)
        {
            Dictionary<int, string> failureDictionary = new Dictionary<int, string>();
            if (failureList != null)
            {
                foreach (ListedEntityFailure failureEntry in failureList)
                {
                    failureDictionary.Add(failureEntry.index, failureEntry.errorMessage);
                }
            }

            return failureDictionary;
        }
    }
}
