﻿// Copyright (c) 2007 James Newton-King. All rights reserved.
// Use of this source code is governed by The MIT License,
// as found in the license.md file.

class BidirectionalDictionary<TFirst, TSecond> where TFirst : notnull where TSecond : notnull
{
    readonly IDictionary<TFirst, TSecond> firstToSecond;
    readonly IDictionary<TSecond, TFirst> secondToFirst;
    readonly string duplicateFirstErrorMessage;
    readonly string duplicateSecondErrorMessage;

    public BidirectionalDictionary()
        : this(EqualityComparer<TFirst>.Default, EqualityComparer<TSecond>.Default)
    {
    }

    public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer)
        : this(
            firstEqualityComparer,
            secondEqualityComparer,
            "Duplicate item already exists for '{0}'.",
            "Duplicate item already exists for '{0}'.")
    {
    }

    public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer,
        string duplicateFirstErrorMessage, string duplicateSecondErrorMessage)
    {
        firstToSecond = new Dictionary<TFirst, TSecond>(firstEqualityComparer);
        secondToFirst = new Dictionary<TSecond, TFirst>(secondEqualityComparer);
        this.duplicateFirstErrorMessage = duplicateFirstErrorMessage;
        this.duplicateSecondErrorMessage = duplicateSecondErrorMessage;
    }

    public void Set(TFirst first, TSecond second)
    {
        if (firstToSecond.TryGetValue(first, out var existingSecond))
        {
            if (!existingSecond.Equals(second))
            {
                throw new ArgumentException(string.Format(duplicateFirstErrorMessage, first));
            }
        }

        if (secondToFirst.TryGetValue(second, out var existingFirst))
        {
            if (!existingFirst.Equals(first))
            {
                throw new ArgumentException(string.Format(duplicateSecondErrorMessage, second));
            }
        }

        firstToSecond.Add(first, second);
        secondToFirst.Add(second, first);
    }

    public bool TryGetByFirst(TFirst first, [NotNullWhen(true)] out TSecond? second)
    {
        return firstToSecond.TryGetValue(first, out second);
    }

    public bool TryGetBySecond(TSecond second, [NotNullWhen(true)] out TFirst? first)
    {
        return secondToFirst.TryGetValue(second, out first);
    }
}