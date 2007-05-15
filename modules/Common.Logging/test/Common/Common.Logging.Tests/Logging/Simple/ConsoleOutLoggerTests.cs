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

using System;
using System.Collections.Specialized;
using NUnit.Framework;

namespace Common.Logging.Simple
{
    /// <summary>
    /// Exercises the ConsoleOutLogger implementation.
    /// </summary>
    /// <author>Mark Pollack</author>
    /// <version>$Id:$</version>
    [TestFixture]
    public class ConsoleOutLoggerTests : AbstractSimpleLogTest
    {
        [SetUp]
        public void Setup()
        {
            NameValueCollection properties = GetProperties();

            // set Adapter
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(properties);
            defaultLogInstance = LogManager.GetLogger(LoggerType.FullName);
        }

        public override Type LoggerType
        {
            get { return typeof (ConsoleOutLogger); }
        }

        /// <summary>
        /// Basic checks specific to ConsoleOutLogger
        /// </summary>
        /// <param name="log">The log.</param>
        protected override void CheckLog(ILog log)
        {
            Assert.IsNotNull(log);
            Assert.IsInstanceOfType(LoggerType, log);

            // Can we call level checkers with no exceptions?
            // Note that everything is hard-coded to be disabled for NoOpLogger
            Assert.IsTrue(log.IsDebugEnabled);
            Assert.IsTrue(log.IsInfoEnabled);
            Assert.IsTrue(log.IsWarnEnabled);
            Assert.IsTrue(log.IsErrorEnabled);
            Assert.IsTrue(log.IsFatalEnabled);
        }
    }
}