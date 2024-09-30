// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using TMPro;
using UnityEngine;

/// <summary>
/// This is the FPS Checker of Unity itself, 
/// NOT the update frequency of the result by MediaPipe
/// </summary>
public class FPSChecker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _fpsValueText;

    private int _frameCount = 0;
    private float _elapsedTime = 0.0f;

    [SerializeField] private float _updateInterval = 0.5f; // [sec]

    void Update()
    {
        _frameCount++;
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _updateInterval)
        {
            float fps = _frameCount / _elapsedTime;

            _fpsValueText.text = fps.ToString("0.0");

            _frameCount = 0;
            _elapsedTime = 0.0f;
        }
    }
}
