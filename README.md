# UniTime

[![test](https://github.com/chikacc/UniTime/actions/workflows/test.yml/badge.svg)](https://github.com/chikacc/UniTime/actions/workflows/test.yml)
[![openupm](https://img.shields.io/npm/v/com.chikacc.unitime?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.chikacc.unitime)
[![releases](https://img.shields.io/github/v/release/chikacc/UniTime?display_name=tag&include_prereleases&sort=semver)](https://github.com/chikacc/UniTime/releases)
[![release data](https://img.shields.io/github/release-date-pre/chikacc/UniTime)](https://github.com/chikacc/UniTime/releases)
![unity](https://img.shields.io/badge/unity-2021.3%20or%20later-green)
[![license](https://img.shields.io/github/license/chikacc/UniTime)](https://github.com/chikacc/UniTime/blob/master/LICENSE.md)

Provides a testable time integration for Unity.

## Overview

The UniTime Package provides a testable time integration solution for Unity, allowing developers to easily manipulate and control time within their projects. With UniTime, you can pause, slow down, or speed up time, as well as create custom time events and triggers. This package is perfect for games that require advanced time management, such as simulations, strategy games, or puzzle games.

## Gettings Started

Install via [UPM package]() or asset package(UniTime.*.*.*.unitypackage) available in [UniTime/releases]() page.

```cs
using UniTime;

async Task Demo() {
    var timeProvider = new UniTimeProvider();
    Debug.Log("Current time: " + timeProvider.Time);

    var stopwatch = new DoubleBasedUniTimeStopwatch(timeProvider);
    stopwatch.Start();

    var ticker = new GameObject().AddComponent<CoroutineUniTimeTicker>();
    ticker.Tick += () => Debug.Log($"Ticker tick: {timeProvider.Time}");

    var timer = new DoubleBasedUniTimeTimer(timeProvider, ticker);
    timer.Interval = 5000; // 5 seconds
    timer.AutoReset = true;
    timer.Elapsed += (sender, e) => Debug.Log($"Timer elapsed: {stopwatch.Elapsed}");
    timer.Enabled = true;
    
    await Task.Delay(TimeSpan.FromSeconds(15));
    stopwatch.Stop();
    Debug.Log("Stopwatch elapsed: " + stopwatch.Elapsed);
}
```

## License

This library is licensed under the MIT License.

Logo icon made by verry purnomo from Flaticon.
Logo Font made by SourceSansPro from Adobe Systems Incorporated.
