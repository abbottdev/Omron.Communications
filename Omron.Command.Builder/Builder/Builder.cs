//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Omron.Commands.Builder
//{
//    public sealed class Builder : IBuilderExpression
//    {
//        private static IBuilderExpression _instance;

//        private IReadCommandBuilder _read;


//        public static IBuilderExpression Factory()
//        {
//            if (_instance == null)
//                _instance = new Builder();

//            return _instance;
//        }
//        public Builder()
//        {

//        }

//        public Builder(IReadCommandBuilder readBuilder)
//        {
//            _read = readBuilder;
//        }

//        public IReadCommandBuilder ForRead()
//        {
//            return _read;
//        }

//    }
//}
