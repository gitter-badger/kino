﻿using System;

namespace rawf.Framework
{
    public static class ExceptionExtensions
    {
        public static bool OperationCanceled(this Exception err)
        {
            return err is OperationCanceledException;
        }
    }
}