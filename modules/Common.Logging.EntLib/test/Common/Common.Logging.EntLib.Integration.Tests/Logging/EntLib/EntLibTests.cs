#region License

/*
 * Copyright � 2002-2007 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Imports

using System;
using System.Diagnostics;
using Common.Logging.Simple;
using Common.TestUtil;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using NUnit.Framework;

#endregion

namespace Common.Logging.EntLib
{
    /// <summary>
    /// Test for the EntLib implementation of ILog 
    /// </summary>
    /// <author>Mark Pollack</author>
    [TestFixture]
    public class EntLibTests : AbstractSimpleLoggerTestsBase
    {
        protected override ILoggerFactoryAdapter GetLoggerFactoryAdapter()
        {
            return new EntLibLoggerFactoryAdapter();
        }

        protected override string CompliantTrustLevelName
        {
            get
            {
                return SecurityUtil.PERMISSIONSET_FULLTRUST;
            }
        }

        [Test]
        public void AssertDefaultSettings()
        {
            ILog log = LogManager.GetCurrentClassLogger();
            Assert.IsNotNull(log);
            Assert.IsInstanceOf<EntLibLogger>(log);

            // Can we call level checkers with no exceptions?
            Assert.IsTrue(log.IsDebugEnabled);
            Assert.IsTrue(log.IsInfoEnabled);
            Assert.IsTrue(log.IsWarnEnabled);
            Assert.IsTrue(log.IsErrorEnabled);
            Assert.IsTrue(log.IsFatalEnabled);
        }
    }
}