using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIContainer: IDisposable {
	private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

	public void Dispose() {
		foreach (var service in services.Values) {
			if (service is IDisposable disposableService) {
				disposableService.Dispose();
			}
		}
		services.Clear();
	}

	public void Register<T>(T instance) {
		var type = typeof(T);
		if (services.ContainsKey(type))
			Debug.LogWarning($"Service {type} is already registered, overwriting.");

		services[type] = instance;
	}
	public T Resolve<T>() {
		if (services.TryGetValue(typeof(T), out var instance))
			return (T)instance;

		throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");
	}
}
