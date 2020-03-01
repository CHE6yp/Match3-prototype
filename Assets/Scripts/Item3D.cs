using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item3D : MonoBehaviour, IItemVisual
{
    public Item item { get; set; }
    public new Transform transform { get; private set; }

    public static Item3D selectedButton;
    public float interval = 3;
    public TextMesh debugText;

    private void Update()
    {
        debugText.text = item.coordinates.y.ToString() + '\n' + (transform.position.y/interval - item.coordinates.y);
    }

    public void Setup(Item item)
    {
        transform = GetComponent<Transform>();
        this.item = item;
        item.visualObject = this;

        UpdatePosition();

        UpdateLook();

        //buttonComponent.onClick.AddListener(Select);

        //item.droppedTo += (coordinates) => { Drop(coordinates); };
        //item.scored += Score;
    }

    public void UpdateLook()
    {
        int i = 0;
        switch (item.type)
        {
            default:
            case "A": i=0; break;
            case "B": i=1; break;
            case "C": i=2; break;
            case "D": i=3; break;
            case "E": i=4; break;
        }

        transform.GetChild(0).GetComponent<MeshFilter>().mesh = UIManager.instance.buttonMeshes[i];
        transform.GetChild(0).GetComponent<MeshRenderer>().material = UIManager.instance.buttonMaterials[i];

        transform.GetChild(1).GetComponent<ParticleSystem>().startColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
        transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().startColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
    }
    
    public void UpdatePosition()
    {
        transform.position = item.coordinates * interval;
    }

    //not really used, but let's leave it
    public void MoveTo(Vector2 coordinates)
    {
        transform.position = coordinates * interval;
    }

    /// <summary>
    /// Процедурно создаем анимацию падения предмета в зависимости от высоты. И дропаем
    /// </summary>
    public IEnumerator Drop()
    {
        Debug.Log("Drop! "+ (transform.position.y / interval - item.coordinates.y).ToString());
        //Animator animator = GetComponent<Animator>();
        Animation anim = GetComponent<Animation>();
        //StartCoroutine(Drop(animator));

        AnimationCurve curve;

        AnimationClip clip = new AnimationClip();
        clip.legacy = true;

        Keyframe[] keys;
        keys = new Keyframe[2];
        keys[0] = new Keyframe(0.0f, transform.position.y);
        keys[1] = new Keyframe(0.5f, item.coordinates.y*interval);
        //keys[2] = new Keyframe(2.0f, 0.0f);
        curve = new AnimationCurve(keys);
        clip.SetCurve("", typeof(Transform), "localPosition.x", AnimationCurve.Constant(0,0,transform.position.x));
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve);

        anim.AddClip(clip, "tmp");
        anim.Play("tmp");
        //need to wait for all animations
        while (anim.isPlaying)
            yield return null;

        GameManager.instance.dropCounter--;

        //anim.RemoveClip("tmp");
        //anim.AddClip(clip, clip.name);
        //anim.Play(clip.name);

    }

    public void Select()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (selectedButton != null) selectedButton.Deselect();
            //todo change gamemanage.inst.SelectItem(item) to item.Select();
            if (!GameManager.instance.SelectItem(item))
            {
                selectedButton = this;
                transform.GetChild(1).GetComponent<ParticleSystem>().Play(true);
            }
            else
            {
                selectedButton = null;
            }
            return;
        }
        selectedButton = this;
        transform.GetChild(1).GetComponent<ParticleSystem>().Play(true);
    }

    public void Deselect()
    {
        selectedButton = null;
        transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
    }

    public void Score()
    {
        transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Play(true);
    }
}
