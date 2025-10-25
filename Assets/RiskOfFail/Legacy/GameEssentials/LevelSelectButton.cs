using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private Menu_Infobox menuBox;
    [SerializeField] private LevelSelectItem level;
    [SerializeField] private TextMeshProUGUI name;
    public RectTransform selectorSprite;

    private void Start()
    {
        name.SetText(level.name);

        GetComponent<Button>().onClick.AddListener(EnterLoadoutSelect);
    }

    private void EnterLoadoutSelect()
    {
        if (level.followLoadout)
        {
            LoadoutSelectManager.instance.isSelectingLoadout = true;

            LoadoutSelectManager.instance.startButton.onClick.RemoveAllListeners();
            LoadoutSelectManager.instance.startButton.onClick.AddListener(() =>
            {
                LoadoutSelectManager.instance.startButton.onClick.RemoveAllListeners();
                LevelFade.FadeIn(() =>
                {
                    if (!level.followLoadout) Destroy(LoadoutManager.instance.gameObject);

                    SceneManager.LoadScene(level.buildIndex);
                    LevelFade.FadeOut();
                });
            });
        }
        else
        {
            LevelFade.FadeIn(() =>
            {
                SceneManager.LoadScene(level.buildIndex);
                LevelFade.FadeOut();
            });
        }
    }

    public void MouseEnter(BaseEventData b)
    {
        if (LoadoutSelectManager.instance.isSelectingLoadout) return;

        SelectLevel(.1f);
    }

    public void SelectLevel(float t)
    {
        menuBox.SetLevel(level);
        menuBox.ShowLevel();

        selectorSprite.DOMove(transform.position, t);
    }
}