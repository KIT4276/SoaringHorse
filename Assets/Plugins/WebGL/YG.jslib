mergeInto(LibraryManager.library, {
    YG_Init: function (goNamePtr) {
        try {
            var goName = UTF8ToString(goNamePtr);
            console.log("[YG_Init] called, goName =", goName);

            window.__ygBridge = window.__ygBridge || {
                goName: null,
                ysdk: null,
                player: null,
                initialized: false,
                send: null
            };

            var bridge = window.__ygBridge;
            bridge.goName = goName;

            function send(method, value) {
                try {
                    var payload = (value === undefined || value === null) ? "" : String(value);

                    console.log("[YG->Unity] send attempt:", {
                        goName: bridge.goName,
                        method: method,
                        value: payload
                    });

                    if (typeof SendMessage === "function") {
                        SendMessage(bridge.goName, method, payload);
                        console.log("[YG->Unity] sent via global SendMessage:", method);
                        return;
                    }

                    if (typeof unityInstance !== "undefined" && unityInstance && unityInstance.SendMessage) {
                        unityInstance.SendMessage(bridge.goName, method, payload);
                        console.log("[YG->Unity] sent via unityInstance.SendMessage:", method);
                        return;
                    }

                    if (typeof window !== "undefined" &&
                        window.unityInstance &&
                        window.unityInstance.SendMessage) {
                        window.unityInstance.SendMessage(bridge.goName, method, payload);
                        console.log("[YG->Unity] sent via window.unityInstance.SendMessage:", method);
                        return;
                    }

                    console.warn("[YG->Unity] SendMessage unavailable:", {
                        goName: bridge.goName,
                        method: method,
                        value: payload
                    });
                } catch (e) {
                    console.error("[YG->Unity] SendMessage error:", method, e);
                }
            }

            bridge.send = send;

            if (bridge.initialized) {
                var cachedLang = "en";

                console.log("[YG_Init] bridge already initialized");
                send("OnYsdkInitOk", "");

                if (bridge.ysdk &&
                    bridge.ysdk.environment &&
                    bridge.ysdk.environment.i18n &&
                    bridge.ysdk.environment.i18n.lang) {
                    cachedLang = bridge.ysdk.environment.i18n.lang;
                }

                send("OnLanguageDetected", cachedLang);
                send("OnPlayerReady", bridge.player ? "1" : "0");
                return;
            }

            if (typeof YaGames === "undefined" || !YaGames.init) {
                console.error("[YG_Init] YaGames SDK not found");
                send("OnYsdkInitError", "YaGames SDK not found");
                return;
            }

            console.log("[YG_Init] YaGames.init() start");

            YaGames.init()
                .then(function (ysdk) {
                    var lang = "en";

                    console.log("[YG_Init] YaGames.init() success", ysdk);

                    bridge.ysdk = ysdk;
                    bridge.initialized = true;

                    send("OnYsdkInitOk", "");

                    if (ysdk &&
                        ysdk.environment &&
                        ysdk.environment.i18n &&
                        ysdk.environment.i18n.lang) {
                        lang = ysdk.environment.i18n.lang;
                    }

                    console.log("[YG_Init] detected language =", lang);
                    send("OnLanguageDetected", lang);

                    if (ysdk.features && ysdk.features.GameplayAPI) {
                        try {
                            console.log("[YG_Init] subscribe GameplayAPI pause/resume");

                            ysdk.features.GameplayAPI.on("pause", function () {
                                console.log("[YG GameplayAPI] pause");
                                send("OnGameApiPause", "");
                            });

                            ysdk.features.GameplayAPI.on("resume", function () {
                                console.log("[YG GameplayAPI] resume");
                                send("OnGameApiResume", "");
                            });
                        } catch (e) {
                            console.warn("[YG_Init] GameplayAPI subscribe failed:", e);
                        }
                    } else {
                        console.warn("[YG_Init] GameplayAPI unavailable");
                    }

                    if (ysdk.getPlayer) {
                        console.log("[YG_Init] getPlayer() start");
                        return ysdk.getPlayer({ scopes: false });
                    }

                    console.warn("[YG_Init] ysdk.getPlayer unavailable");
                    return null;
                })
                .then(function (player) {
                    console.log("[YG_Init] getPlayer() resolved, hasPlayer =", !!player);

                    bridge.player = player || null;
                    send("OnPlayerReady", player ? "1" : "0");
                })
                .catch(function (e) {
                    console.error("[YG_Init] error:", e);
                    send("OnYsdkInitError", e && e.message ? e.message : String(e));
                });
        } catch (e) {
            console.error("[YG_Init] fatal error:", e);
        }
    },

    YG_Ready: function () {
        try {
            console.log("[YG_Ready] called");

            var bridge = window.__ygBridge;
            if (!bridge) {
                console.warn("[YG_Ready] bridge unavailable");
                return;
            }

            if (!bridge.ysdk) {
                console.warn("[YG_Ready] SDK not initialized");
                return;
            }

            if (bridge.ysdk.features &&
                bridge.ysdk.features.LoadingAPI &&
                bridge.ysdk.features.LoadingAPI.ready) {
                console.log("[YG_Ready] LoadingAPI.ready()");
                bridge.ysdk.features.LoadingAPI.ready();
            } else {
                console.warn("[YG_Ready] LoadingAPI.ready unavailable");
            }
        } catch (e) {
            console.error("[YG_Ready] error:", e);
        }
    },

    YG_ShowFullscreenAdv: function () {
        try {
            console.log("[YG_ShowFullscreenAdv] called");

            var bridge = window.__ygBridge;
            if (!bridge) {
                console.warn("[YG_ShowFullscreenAdv] bridge unavailable");
                return;
            }

            if (!bridge.ysdk || !bridge.ysdk.adv) {
                console.warn("[YG_ShowFullscreenAdv] adv unavailable");
                if (bridge.send) {
                    bridge.send("OnAdError", "Fullscreen adv unavailable");
                }
                return;
            }

            bridge.ysdk.adv.showFullscreenAdv({
                callbacks: {
                    onOpen: function () {
                        console.log("[YG_ShowFullscreenAdv] onOpen");
                        bridge.send("OnAdOpen", "");
                    },
                    onClose: function (wasShown) {
                        console.log("[YG_ShowFullscreenAdv] onClose, wasShown =", wasShown);
                        bridge.send("OnAdClose", wasShown ? "1" : "0");
                    },
                    onError: function (e) {
                        console.error("[YG_ShowFullscreenAdv] onError:", e);
                        bridge.send("OnAdError", e && e.message ? e.message : String(e));
                    },
                    onOffline: function () {
                        console.warn("[YG_ShowFullscreenAdv] onOffline");
                        bridge.send("OnAdError", "Offline");
                    }
                }
            });
        } catch (e) {
            console.error("[YG_ShowFullscreenAdv] fatal error:", e);
            var bridge = window.__ygBridge;
            if (bridge && bridge.send) {
                bridge.send("OnAdError", e && e.message ? e.message : String(e));
            }
        }
    },

    YG_ShowRewardedVideo: function () {
        try {
            console.log("[YG_ShowRewardedVideo] called");

            var bridge = window.__ygBridge;
            if (!bridge) {
                console.warn("[YG_ShowRewardedVideo] bridge unavailable");
                return;
            }

            if (!bridge.ysdk || !bridge.ysdk.adv) {
                console.warn("[YG_ShowRewardedVideo] adv unavailable");
                if (bridge.send) {
                    bridge.send("OnRvError", "Rewarded adv unavailable");
                }
                return;
            }

            bridge.ysdk.adv.showRewardedVideo({
                callbacks: {
                    onOpen: function () {
                        console.log("[YG_ShowRewardedVideo] onOpen");
                        bridge.send("OnRvOpen", "");
                    },
                    onRewarded: function () {
                        console.log("[YG_ShowRewardedVideo] onRewarded");
                        bridge.send("OnRvReward", "");
                    },
                    onClose: function () {
                        console.log("[YG_ShowRewardedVideo] onClose");
                        bridge.send("OnRvClose", "");
                    },
                    onError: function (e) {
                        console.error("[YG_ShowRewardedVideo] onError:", e);
                        bridge.send("OnRvError", e && e.message ? e.message : String(e));
                    }
                }
            });
        } catch (e) {
            console.error("[YG_ShowRewardedVideo] fatal error:", e);
            var bridge = window.__ygBridge;
            if (bridge && bridge.send) {
                bridge.send("OnRvError", e && e.message ? e.message : String(e));
            }
        }
    },

    YG_PlayerGetData: function () {
        try {
            console.log("[YG_PlayerGetData] called");

            var bridge = window.__ygBridge;
            if (!bridge || !bridge.send) {
                console.warn("[YG_PlayerGetData] bridge unavailable");
                return;
            }

            if (bridge.player && bridge.player.getData) {
                console.log("[YG_PlayerGetData] loading from cloud player.getData()");
                bridge.player.getData()
                    .then(function (data) {
                        var json = JSON.stringify(data || {});
                        console.log("[YG_PlayerGetData] cloud success, json length =", json.length);
                        bridge.send("OnCloudData", json);
                    })
                    .catch(function (e) {
                        console.error("[YG_PlayerGetData] cloud error:", e);
                        bridge.send("OnCloudData", "");
                    });
                return;
            }

            console.warn("[YG_PlayerGetData] player.getData unavailable, fallback to localStorage");

            try {
                var local = localStorage.getItem("YG_PlayerData");
                console.log("[YG_PlayerGetData] localStorage success, hasData =", !!local);
                bridge.send("OnCloudData", local || "");
            } catch (e) {
                console.error("[YG_PlayerGetData] localStorage error:", e);
                bridge.send("OnCloudData", "");
            }
        } catch (e) {
            console.error("[YG_PlayerGetData] fatal error:", e);
        }
    },

    YG_PlayerSetData: function (jsonPtr) {
        try {
            var json = UTF8ToString(jsonPtr);
            console.log("[YG_PlayerSetData] called, json length =", json ? json.length : 0);

            var bridge = window.__ygBridge;

            if (bridge && bridge.player && bridge.player.setData) {
                var parsedData = {};

                try {
                    parsedData = json ? JSON.parse(json) : {};
                    console.log("[YG_PlayerSetData] JSON parsed successfully");
                } catch (e) {
                    console.error("[YG_PlayerSetData] JSON parse error:", e);
                    if (bridge && bridge.send) {
                        bridge.send("OnCloudSaveError", e && e.message ? e.message : String(e));
                    }
                    return;
                }

                console.log("[YG_PlayerSetData] saving to cloud player.setData()");
                bridge.player.setData(parsedData)
                    .then(function () {
                        console.log("[YG_PlayerSetData] cloud save success");
                        if (bridge.send) {
                            bridge.send("OnCloudSaveOk", "");
                        }
                    })
                    .catch(function (e) {
                        console.error("[YG_PlayerSetData] cloud save error:", e);
                        if (bridge.send) {
                            bridge.send("OnCloudSaveError", e && e.message ? e.message : String(e));
                        }
                    });

                return;
            }

            console.warn("[YG_PlayerSetData] player.setData unavailable, fallback to localStorage");

            try {
                localStorage.setItem("YG_PlayerData", json || "");
                console.log("[YG_PlayerSetData] localStorage save success");
                if (bridge && bridge.send) {
                    bridge.send("OnCloudSaveOk", "");
                }
            } catch (e) {
                console.error("[YG_PlayerSetData] localStorage error:", e);
                if (bridge && bridge.send) {
                    bridge.send("OnCloudSaveError", e && e.message ? e.message : String(e));
                }
            }
        } catch (e) {
            console.error("[YG_PlayerSetData] fatal error:", e);
            var bridge = window.__ygBridge;
            if (bridge && bridge.send) {
                bridge.send("OnCloudSaveError", e && e.message ? e.message : String(e));
            }
        }
    }
});