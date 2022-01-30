
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityUtils {
    public static T OrNull<T>(this T obj) where T : UnityEngine.Object {
        return obj == null ? null : obj;
    }
}
