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

namespace EckID.SCrypter
{
    static class Constants
    {
        public static string SALT = "rktYml0MIp9TC9u6Ny6uqw==";
        public static int N = 131072;
        public static int r = 8;
        public static int p = 4;
        public static int? MAX_THREADS = null;
        public static int DERIVED_KEY_LENGTH = 32;
    }
}
