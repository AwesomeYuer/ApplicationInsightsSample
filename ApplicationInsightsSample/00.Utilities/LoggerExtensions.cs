#if !NETFRAMEWORK
namespace Microshaoft
{
    using Microsoft.Extensions.Logging;
    using System;
    public static class LoggerExtensions
    {
        public static void LogOnDemand
                (
                   this ILogger @this
                    , LogLevel logLevel
                    , Action processAction
                )
        {
            if (@this.IsEnabled(logLevel))
            {
                processAction();
            }
        }
        public static void LogOnDemand
                (
                   this ILogger @this
                    , LogLevel logLevel
                    , Func<string> messageFactory
                )
        {
            var enums = Enum.GetValues(typeof(LogLevel));
            foreach (LogLevel e in enums)
            {
                Console.WriteLine($"{e}: {@this.IsEnabled(e)}");
            }

            var loggingFormatArguments = new object[] { };
            @this
                .LogOnDemand
                    (
                        logLevel
                        , messageFactory
                        , loggingFormatArguments
                    );
        }
        public static void LogOnDemand
                                (
                                   this ILogger @this
                                    , LogLevel logLevel
                                    , Func<string> messageFactory
                                    , params object[] loggingFormatArguments
                                )
        {
            if (@this.IsEnabled(logLevel))
            {
                var message = messageFactory();
                @this
                    .Log
                        (
                            logLevel
                            , message
                            , loggingFormatArguments
                        );
            }
        }
        public static void LogOnDemand
                                (
                                   this ILogger @this
                                    , LogLevel logLevel
                                    , Exception exception
                                    , Func<string> messageFactory

                                )
        {
            var loggingFormatArguments = new object[] { };
            @this
                .LogOnDemand
                    (
                        logLevel
                        , exception
                        , messageFactory
                        , loggingFormatArguments
                    );
        }
        public static void LogOnDemand
                                (
                                   this ILogger @this
                                    , LogLevel logLevel
                                    , Exception exception
                                    , Func<string> messageFactory
                                    , params object[] loggingFormatArguments
                                )
        {
            if (@this.IsEnabled(logLevel))
            {
                var message = messageFactory();
                @this
                    .Log
                        (
                            logLevel
                            , exception
                            , message
                            , loggingFormatArguments
                        );
            }
        }

        public static void LogOnDemand
                         (
                            this ILogger @this
                             , LogLevel logLevel
                             , EventId eventId
                             , Exception exception
                             , Func<string> messageFactory
                         )
        {
            var loggingFormatArguments = new object[] { };
            @this
                .LogOnDemand
                    (
                        logLevel
                        , eventId
                        , exception
                        , messageFactory
                        , loggingFormatArguments
                    );
        }
        public static void LogOnDemand
                                (
                                   this ILogger @this
                                    , LogLevel logLevel
                                    , EventId eventId
                                    , Exception exception
                                    , Func<string> messageFactory
                                    , params object[] loggingFormatArguments
                                )
        {
            if (@this.IsEnabled(logLevel))
            {
                var message = messageFactory();
                @this
                    .Log
                        (
                            logLevel
                            , eventId
                            , exception
                            , message
                            , loggingFormatArguments
                        );
            }
        }
        public static void LogOnDemand
                         (
                            this ILogger @this
                             , LogLevel logLevel
                             , EventId eventId
                             , Func<string> messageFactory
                         )
        {
            var loggingFormatArguments = new object[] { };
            @this
                .LogOnDemand
                    (
                        logLevel
                        , eventId
                        , messageFactory
                        , loggingFormatArguments
                    );
        }
        public static void LogOnDemand
                                (
                                   this ILogger @this
                                    , LogLevel logLevel
                                    , EventId eventId
                                    , Func<string> messageFactory
                                    , params object[] loggingFormatArguments
                                )
        {
            if (@this.IsEnabled(logLevel))
            {
                var message = messageFactory();
                @this
                    .Log
                        (
                            logLevel
                            , eventId
                            , message
                            , loggingFormatArguments
                        );
            }
        }

        public static void LogOnDemand
                        (
                            this ILogger @this
                            , LogLevel logLevel
                            , Func
                                <
                                    (
                                        EventId loggingEventId
                                        , Exception loggingException
                                        , string loggingMessage
                                        , object[] loggingFormatArguments
                                    )
                                > loggingPreprocess
                        )
        {
            if (@this.IsEnabled(logLevel))
            {
                var (loggingEventId, loggingException, loggingMessage, loggingFormatArguments) = loggingPreprocess();
                @this
                    .Log
                        (
                            logLevel
                            , loggingEventId
                            , loggingException
                            , loggingMessage
                            , loggingFormatArguments
                        );
            }
        }
        public static void LogOnDemand
                        (
                            this ILogger @this
                            , LogLevel logLevel
                            , Func
                                <
                                    (
                                        EventId loggingEventId
                                        , string loggingMessage
                                        , object[] loggingFormatArguments
                                    )
                                > loggingPreprocess
                        )
        {
            if (@this.IsEnabled(logLevel))
            {
                var (loggingEventId, loggingMessage, loggingFormatArguments) = loggingPreprocess();
                @this
                    .Log
                        (
                            logLevel
                            , loggingEventId
                            , loggingMessage
                            , loggingFormatArguments
                        );
            }
        }
        public static void LogOnDemand
                        (
                            this ILogger @this
                            , LogLevel logLevel
                            , Func
                                <
                                    (
                                        string loggingMessage
                                        , object[] loggingFormatArguments
                                    )
                                > loggingPreprocess
                        )
        {
            if (@this.IsEnabled(logLevel))
            {
                var (loggingMessage, loggingFormatArguments) = loggingPreprocess();
                @this
                    .Log
                        (
                            logLevel
                            , loggingMessage
                            , loggingFormatArguments
                        );
            }
        }

        public static void LogOnDemand
                (
                    this ILogger @this
                    , LogLevel logLevel
                    , Func
                        <
                            (
                                Exception loggingException
                                , string loggingMessage
                                , object[] loggingFormatArguments
                            )
                        > loggingPreprocess
                )
        {
            if (@this.IsEnabled(logLevel))
            {
                var (
                        loggingException
                        , loggingMessage
                        , loggingFormatArguments
                    ) = loggingPreprocess();
                @this
                    .Log
                        (
                            logLevel
                            , loggingException
                            , loggingMessage
                            , loggingFormatArguments
                        );
            }
        }

        public static void LogOnDemand<TState>
                (
                    this ILogger @this
                    , LogLevel logLevel
                    , Func
                        <
                            (
                                EventId loggingEventId
                                , Exception loggingException
                                , TState State
                            )
                        > loggingPreprocess
                    , Func<TState, Exception, string> formatter
                )
        {
            if (@this.IsEnabled(logLevel))
            {
                var (loggingEventId, loggingException, State) = loggingPreprocess();
                @this
                    .Log
                        (
                            logLevel
                            , loggingEventId
                            , State
                            , loggingException
                            , formatter!
                        );
            }
        }
    }
}
#endif