using System;

namespace SharpBoost {
    public static class Arguments {
        public static Func<string, T> ArgumentCheck<T>(this T argument, Func<T, bool> invalidCondition) {
            return message => {
                ArgumentNullCheck(invalidCondition, "invalidCondition");

                if (invalidCondition(argument))
                    throw new ArgumentException(message.ValOrDefault(String.Empty));

                return argument;
            };
        }

        public static T ArgumentNullCheck<T>(this T argument, string name)
          where T : class {
            if (argument == null)
                throw new ArgumentNullException(name.Return(x => x, String.Empty));
            return argument;
        }

        public static string StringArgumentCheck(this string argument, string name) {
            return ArgumentCheck(argument, String.IsNullOrEmpty)("{0} is null or empty".F(name));
        }
    }
}
