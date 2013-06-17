/*
 *  Pkcs11Interop - Open-source .NET wrapper for unmanaged PKCS#11 libraries
 *  Copyright (c) 2012-2013 JWC s.r.o.
 *  Author: Jaroslav Imrich
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3
 *  as published by the Free Software Foundation.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU Affero General Public License for more details.
 *
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <http://www.gnu.org/licenses/>.
 * 
 *  You can be released from the requirements of the license by purchasing
 *  a commercial license. Buying such a license is mandatory as soon as you
 *  develop commercial activities involving the Pkcs11Interop software without
 *  disclosing the source code of your own applications.
 * 
 *  For more information, please contact JWC s.r.o. at info@pkcs11interop.net
 */

using NUnit.Framework;
using System;
using Net.Pkcs11Interop.HighLevelAPI;

namespace Net.Pkcs11Interop.Tests.HighLevelAPI
{
    /// <summary>
    /// Pkcs11 construct, dispose, initialize and finalize tests.
    /// </summary>
    [TestFixture()]
    public class InitializeTest
    {
        /// <summary>
        /// Basic construct and dispose test.
        /// </summary>
        [Test()]
        public void BasicPkcs11DisposeTest()
        {
            // Unmanaged PKCS#11 library is loaded by the constructor of Pkcs11 class.
            // Every PKCS#11 library needs to be initialized with C_Initialize method
            // which is also called automatically by the constructor of Pkcs11 class.
            Pkcs11 pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath, false);
            
            // Do something  interesting
            
            // Unmanaged PKCS#11 library is unloaded by Dispose() method.
            // C_Finalize should be the last call made by an application and it
            // is also called automatically by Dispose() method.
            pkcs11.Dispose();
        }
        
        /// <summary>
        /// Using statement test.
        /// </summary>
        [Test()]
        public void UsingPkcs11DisposeTest()
        {
            // Pkcs11 class can be used in using statement which defines a scope 
            // at the end of which an object will be disposed.
            using (Pkcs11 pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath, false))
            {
                // Do something interesting
            }
        }

        /// <summary>
        /// Example for single-threaded applications.
        /// </summary>
        [Test()]
        public void SingleThreadedInitializeTest()
        {
            // If an application will not be accessing PKCS#11 library from multiple threads
            // simultaneously, it should specify "false" as a value of "useOsLocking" parameter.
            using (Pkcs11 pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath, false))
            {
                // Do something interesting
            }
        }
        
        /// <summary>
        /// Example for multi-threaded applications.
        /// </summary>
        [Test()]
        public void MultiThreadedInitializeTest()
        {
            // If an application will be accessing PKCS#11 library from multiple threads
            // simultaneously, it should specify "true" as a value of "useOsLocking" parameter.
            // PKCS#11 library will use the native operation system threading model for locking.
            using (Pkcs11 pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath, true))
            {
                // Do something interesting
            }
        }

        /// <summary>
        /// Example for libraries that support C_GetFunctionList()
        /// </summary>
        [Test()]
        public void Pkcs11WithGetFunctionListTest()
        {
            // Before an application can perform any cryptographic operations with Cryptoki library 
            // it has to obtain function pointers for all the Cryptoki API routines present in the library.
            // This can be done either via C_GetFunctionList() function or via platform specific native 
            // function - GetProcAddress() on Windows and dlsym() on Unix.
            // The most simple constructor of Net.Pkcs11Interop.HighLevelAPI.Pkcs11 class uses 
            // C_GetFunctionList() approach but Pkcs11Interop also provides an alternative constructor 
            // that can specify which approach should be used.
            using (Pkcs11 pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath, true, true))
            {
                // Do something interesting
            }
        }

        /// <summary>
        /// Example for libraries that do not support C_GetFunctionList()
        /// </summary>
        [Test()]
        public void Pkcs11WithoutGetFunctionListTest()
        {
            // Before an application can perform any cryptographic operations with Cryptoki library 
            // it has to obtain function pointers for all the Cryptoki API routines present in the library.
            // This can be done either via C_GetFunctionList() function or via platform specific native 
            // function - GetProcAddress() on Windows and dlsym() on Unix.
            // The most simple constructor of Net.Pkcs11Interop.HighLevelAPI.Pkcs11 class uses 
            // C_GetFunctionList() approach but Pkcs11Interop also provides an alternative constructor 
            // that can specify which approach should be used.
            using (Pkcs11 pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath, true, false))
            {
                // Do something interesting
            }
        }
    }
}
