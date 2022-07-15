using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

namespace ColorMixer {

    public class Main : MonoBehaviour, IPointerDownHandler
    {
        public Transform blender;

        public LayerMask mask;

        public GameObject juice;

        public ScenesData scenesData;

        public TextMeshProUGUI levelText, resultText;

        public Image levelImage, cloud, coctail, resultImage, prise;

        public bool isLidOpen = false;

        private Color needColor;
        

        private void Start() {
            StartCoroutine(InitScene());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            RaycastHit hit;
        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit)) {

                string maskName = LayerMask.LayerToName(hit.transform.gameObject.layer);
                Transform jug = blender.transform.Find("Jug");
                Transform lid = jug.transform.Find("Lid");
                Transform foods = jug.transform.Find("Foods");

                if (maskName == "Food") {
                    var Seq = DOTween.Sequence();

                    Seq.Append(lid.transform.DORotate(new Vector3(0,0,-90f), 0.85f).SetRelative().SetInverted());
                    Seq.Join(lid.transform.DOMove(new Vector3(lid.localPosition.x - 0.07f, lid.localPosition.y - 0.18f, 0), 0.85f).SetRelative().SetInverted());
                    
                    Seq.Join(hit.transform.gameObject.transform.DOJump(
                        new Vector3(jug.position.x, jug.position.y, jug.position.z), 0.5f, 1, 1f, false));
                    hit.transform.gameObject.transform.SetParent(foods);
                    Seq.AppendInterval(1f);
                    Seq.Join(jug.transform.DOShakePosition(0.5f, new Vector3(0.01f, 0, 0), 10, 0, false, false));
                    Seq.Join(hit.transform.gameObject.transform.DOShakePosition(0.5f, new Vector3(0.01f, 0.03f, 0), 10, 0, false, false));
                }

                if (maskName == "Mix") {
                    juice.SetActive(true);

                    var Seq = DOTween.Sequence();

                    Seq.Append(jug.transform.DOShakePosition(1f, new Vector3(0.01f, 0, 0), 10, 0, false, false));
                    Seq.Join(juice.transform.DOMove(new Vector3(0, juice.transform.localPosition.y - 0.04f, 0), 2f).SetRelative().SetInverted());

                    Transform[] points = foods.GetComponentsInChildren<Transform>();

                    Color color = ColorHelper.ColorLerp(points); 

                    juice.GetComponent<Renderer>().material.color = color;         

                    float percent = ColorHelper.CompareColors(color, needColor);

                    StartCoroutine(EndSceneUI(percent));
                }
            }
        }

        IEnumerator InitScene()
        {
            levelText.text = "Level " + scenesData.CurrentLevelIndex;
            // Инициализируем текст, прозрачность 0
            levelImage.gameObject.SetActive(true);
            // Текст появляется постепенно
            levelImage.DOFade(1f, 0.5f);
            levelText.DOFade(1f, 0.5f);
            // Текстовая пауза на 1 секунду
            yield return new WaitForSeconds(1f);
            // Текст исчезает
            levelImage.DOFade(0f, 0.5f);
            levelText.DOFade(0f, 0.5f);
            // Уничтожаем объект после затухания
            yield return new WaitForSeconds(0.5f);
            levelImage.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            //needColor = ColorHelper.ColorLerp(scenesData.levels[scenesData.CurrentLevelIndex - 1].foodsLevel);
            cloud.gameObject.SetActive(true);
            cloud.DOFade(1f, 0.5f);

            GameObject food = ObjectPool.SharedInstance.GetPooledObject();
            //Debug.Log(food.name);
            // if (food.name == "banana(Clone)") {
            //     food.transform.position = new Vector3(-2.5f, 0.948f, 5.5f);
            //     food.SetActive(true);
            // }

            if (scenesData.CurrentLevelIndex == 1) {
                coctail.color = new Color(0.72f, 0.765f, 0.679f, 1f);
            }
        }

        IEnumerator EndSceneUI(float percent)
        {
            resultImage.gameObject.SetActive(true);
            resultText.text = percent + "%";

            resultImage.DOFade(1f, 0.5f);
            resultText.DOFade(1f, 0.5f);

            if (percent >= 90) {
                prise.gameObject.SetActive(true);
                 yield return new WaitForSeconds(2f);
                scenesData.NextLevel();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

    }
}
