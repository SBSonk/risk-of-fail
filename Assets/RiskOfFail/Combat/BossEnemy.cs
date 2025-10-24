using UnityEngine;
using UnityEngine.UI;

namespace RiskOfFail.Combat
{
    public class BossEnemy : Alive
    {
        public GameObject healthBarPrefab;

        [SerializeField] private ResultsScreen results;
        private Image healthBar;

        private void Update()
        {
            if (healthBar) healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, .25f);
        }

        private void OnEnable()
        {
            InitializeHealthBar();
        }

        private void InitializeHealthBar()
        {
            healthBar = Instantiate(healthBarPrefab).transform.GetChild(0).Find("Health").GetComponent<Image>();
        }

        protected override void Death(KillFlag flag)
        {
            base.Death(flag);

            results.ShowResults();
        }
    }
}