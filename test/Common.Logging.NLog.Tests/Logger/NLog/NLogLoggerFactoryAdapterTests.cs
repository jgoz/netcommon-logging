#region License

/*
 * Copyright 2002-2009 the original author or authors.
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

using System.Reflection;
using System.Security;
using Common.Logging;
using Common.Logging.NLog;
using Common.TestUtil;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using global::NLog.Layouts;
using LogLevel=NLog.LogLevel;
using LogManager=NLog.LogManager;

namespace Common.Logger.NLog
{
    [TestFixture]
    public class NLogLoggerFactoryAdapterTests : ILogTestsBase
    {
        private class TestLoggingConfigurationWithStacktrace : LoggingConfiguration
        {
            public readonly TestTarget Target;

            public TestLoggingConfigurationWithStacktrace()
            {
                Target = new TestTarget();
                Target.Layout = Layout.FromString("${stackTrace}");
                LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, Target));
            }
        }

        private class TestLoggingConfiguration : LoggingConfiguration
        {
            public readonly TestTarget Target;

            public TestLoggingConfiguration()
            {
                Target = new TestTarget();
                Target.Layout = Layout.FromString("${message}");
                LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, Target));
            }
        }

        private sealed class TestTarget : TargetWithLayout
        {
            public LogEventInfo LastLogEvent;
            protected override void Write(LogEventInfo logEvent)
            {
                LastLogEvent = logEvent;
            }
        }

        /// <summary>
        /// NLog lacks <see cref="AllowPartiallyTrustedCallersAttribute"/> 
        /// and therefore needs full trust enviroments.
        /// </summary>
        protected override string CompliantTrustLevelName
        {
            get
            {
                return SecurityTemplate.PERMISSIONSET_FULLTRUST;
            }
        }

        protected override ILoggerFactoryAdapter GetLoggerFactoryAdapter()
        {
            return new NLogLoggerFactoryAdapter(null);
        }

        [Test]
        public void LogsUserStackFrameIfStackTraceRendererIsUsed()
        {
            TestLoggingConfigurationWithStacktrace cfg = new TestLoggingConfigurationWithStacktrace();
            LogManager.Configuration = cfg;

            Common.Logging.LogManager.Adapter = new NLogLoggerFactoryAdapter(null);
            Common.Logging.LogManager.GetLogger("myLogger").Debug("TestMessage");

            Assert.IsNotNull(cfg.Target.LastLogEvent);
            string stackTrace = cfg.Target.LastLogEvent.StackTrace.ToString();
            Assert.AreSame(MethodBase.GetCurrentMethod(), cfg.Target.LastLogEvent.UserStackFrame.GetMethod());
        }

        [Test]
        public void DoesNotLogStackFrameIfStackTraceRendererNotUsed()
        {
            TestLoggingConfiguration cfg = new TestLoggingConfiguration();
            LogManager.Configuration = cfg;

            Common.Logging.LogManager.Adapter = new NLogLoggerFactoryAdapter(null);
            Common.Logging.LogManager.GetLogger("myLogger").Debug("TestMessage");

            Assert.IsNotNull(cfg.Target.LastLogEvent);
            Assert.IsFalse(cfg.Target.LastLogEvent.HasStackTrace);
            Assert.IsNull(cfg.Target.LastLogEvent.StackTrace);
        }
    }
}
