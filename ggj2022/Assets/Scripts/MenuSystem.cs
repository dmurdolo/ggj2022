using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuSystem : MonoBehaviour {
    public Conversation CurrentConversation;
    public Conversation.Path CurrentPath;
    public Conversation.Entry CurrentEntry;
    public bool ShowEntryOptions = false;

    public bool DebugAdvance = false;
    public int DebugSelectOption = -1;

    public bool IsInMenu => CurrentConversation != null;

    public int HighlightedOption = 0;

    public GUIStyle Style;
    public GameObject ConversationCanvasPrefab;
    public ConversationUI ConversationUI;

    public static MenuSystem Instance;

    protected void OnEnable() {
        Instance = this;
        if (!ConversationUI) {
            ConversationUI = Instantiate(ConversationCanvasPrefab).GetComponent<ConversationUI>();
            ConversationUI.gameObject.transform.parent = this.gameObject.transform;
            ConversationUI.gameObject.SetActive(false);

            ConversationUI.Option1.onClick.AddListener(() => DoSelectOption(0));
            ConversationUI.Option1.onClick.AddListener(() => DoSelectOption(1));
            ConversationUI.Option1.onClick.AddListener(() => DoSelectOption(2));
        }
        StopConversation();
    }

    protected void Update() {
        if (DebugAdvance) {
            DebugAdvance = false;
            AdvanceConversation();
        }
        if (DebugSelectOption >= 0) {
            int selectedOption = DebugSelectOption;
            DebugSelectOption = -1;
            DoSelectOption(selectedOption);
        }

        if (ConversationUI) {
            ConversationUI.gameObject.SetActive(false);
        }
        if (CurrentConversation == null || CurrentPath == null) {
            return;
        }
        bool uiVisible = false;
        if (CurrentEntry != null) {
            ConversationUI.MainText.text = CurrentEntry.Text;
            uiVisible = true;
            (UnityEngine.UI.Button, UnityEngine.UI.Text)[] buttons = {
                (ConversationUI.Option1, ConversationUI.OptionText1),
                (ConversationUI.Option2, ConversationUI.OptionText2),
                (ConversationUI.Option3, ConversationUI.OptionText3),
            };
            foreach (var (button, _) in buttons) {
                if (button) {
                    button.gameObject.SetActive(false);
                }
            }
            if (ShowEntryOptions) {
                int index = 0;
                foreach (var option in CurrentPath.Options) {
                    UnityEngine.UI.Button button = null;
                    UnityEngine.UI.Text buttonText = null;
                    if (index < buttons.Length) {
                        (button, buttonText) = buttons[index];
                    } else {
                        break;
                    }

                    if (button && buttonText) {
                        button.gameObject.SetActive(true);
                        string text = option.Text;
                        if (index == HighlightedOption) {
                            button.Select();
                        }
                        buttonText.text = text;
                    }
                    index++;
                }
            }
        }
        ConversationUI.gameObject.SetActive(uiVisible);
    }

    private void DoSelectOption(int selectedOption) {
        if (!CurrentConversation || CurrentPath == null || !ShowEntryOptions) {
            return;
        }
        if (selectedOption < 0 || selectedOption >= CurrentPath.Options.Count) {
            return;
        }
        var option = CurrentPath.Options[selectedOption];

        var storyStateManager = FindObjectOfType<StoryStateManager>();
        if (option.AddStates != null) {
            foreach (var state in option.AddStates) {
                storyStateManager.AddState(state);
            }
        }
        if (option.RemoveStates != null) {
            foreach (var state in option.RemoveStates) {
                storyStateManager.RemoveState(state);
            }
        }

        string nextPathName = option.Path;
        var nextPath = CurrentConversation.Paths.FirstOrDefault(path => path.Name == nextPathName);
        if (nextPath == null) {
            StopConversation();
            return;
        }
        CurrentPath = nextPath;
        CurrentEntry = CurrentPath.Entries.FirstOrDefault();
        ShowEntryOptions = CurrentPath.Entries.Count <= 1 && CurrentPath.Options?.Count > 0;
        if (CurrentPath.Entries.Count == 1) {
            DoSelectOption(0);
        }
    }

    private void AdvanceConversation() {
        if (!CurrentConversation || ShowEntryOptions) {
            return;
        }
        int previousIndex = CurrentPath.Entries.IndexOf(CurrentEntry);
        int nextIndex = previousIndex + 1;
        if (nextIndex >= CurrentPath.Entries.Count) {
            StopConversation();
            return;
        }
        CurrentEntry = CurrentPath.Entries[nextIndex];
        ShowEntryOptions = nextIndex >= CurrentPath.Entries.Count - 1 && CurrentPath.Options?.Count > 0;
        HighlightedOption = 0;
    }

    public void StartConversation(Conversation conversation) {
        StopConversation();
        CurrentConversation = conversation;
        CurrentPath = CurrentConversation.Paths.FirstOrDefault(path => path.Name == "") ?? CurrentConversation.Paths.FirstOrDefault();
        if (CurrentPath == null) {
            return;
        }
        CurrentEntry = CurrentPath.Entries.FirstOrDefault();
        ShowEntryOptions = CurrentPath.Entries.Count <= 1 && CurrentPath.Options?.Count > 0;
    }

    private void StopConversation() {
        CurrentConversation = null;
        CurrentPath = null;
        CurrentEntry = null;
        ShowEntryOptions = false;
        HighlightedOption = 0;
    }

    public void ProcessInput(Vector2? mainAxis = null, bool? accept = null, bool? cancel = null) {
        if (ShowEntryOptions) {
            if (mainAxis != null) {
                int selectDelta = Mathf.RoundToInt(-mainAxis.Value.y + mainAxis.Value.x);
                int optionCount = CurrentPath?.Options?.Count ?? 1;
                HighlightedOption = ((HighlightedOption + selectDelta) + optionCount) % optionCount;
            }
        }
        if (accept == true) {
            if (ShowEntryOptions) {
                DoSelectOption(HighlightedOption);
            } else {
                AdvanceConversation();
            }
        }
    }

    // protected void OnGUI() {
    //     if (CurrentPath == null) {
    //         return;
    //     }
    //     if (CurrentEntry != null) {
    //         GUI.Label(new Rect(0, 0, 300, 100), CurrentEntry.Text, Style);
    //     }
    //     if (ShowEntryOptions) {
    //         int index = 0;
    //         foreach (var option in CurrentPath.Options) {
    //             string text = option.Text;
    //             if (index == HighlightedOption) {
    //                 text = $"[{text}]";
    //             }
    //             GUI.Label(new Rect(30, 50 + index * 30, 300, 50), text, Style);
    //             index++;
    //         }
    //     }
    // }
}
