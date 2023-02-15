using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class FishFactory : MonoBehaviour
{
    [Header("Prefabs")]
    public Fish fish;
    public FishBody[] bodies;
    public float[] bodiesWeights;
    public FishHead[] heads;

    public Sprite[] patternsBodies;
    public Sprite[] patternsHeads;

    public FishBody[] bodiesMini;
    public float[] bodiesMiniWeights;
    public FishHead[] headsMini;

    public Sprite[] patternsBodiesMini;
    public Sprite[] patternsHeadsMini;

    //public Dictionary<BodyID, FishBody> fishBodies = new Dictionary<BodyID, FishBody>();
    //public Dictionary<HeadID, FishHead> fishHeads = new Dictionary<HeadID, FishHead>();

    Fish fishPrefab;

    int layerOrder = 0;

    //Start is called before the first frame update
    void Awake()
    {
        //foreach (FishBody f in bodies)
        //{
        //    fishBodies.Add(f.idBody, f);
        //}
        //foreach (FishHead f in heads)
        //{
        //    fishHeads.Add(f.id, f);
        //}
        fishPrefab = fish;
    }

    //public FishBody GetBody(string id)
    //{
    //    return fishBodies[id];
    //}

    //public FishHead GetHead(string id)
    //{
    //    return fishHeads[id];
    //}

    //public Fish CreateFish(FishIDs ids, Transform parent)
    //{
    //    Fish f = Instantiate(fishPrefab, parent);
    //    f.SetBody(ids.body + " " + ids.pattern);
    //    f.SetHead(ids.head);
    //    return f;
    //}

    public int CountBodies () {
        return bodies.Length;
    }

    public int CountHeads() {
        return heads.Length;
    }

    public int CountPatterns() {
        return patternsBodies.Length / CountBodies();
    }

    public FishBody GetBodyById(int idBody, int idPattern) {
        FishBody body = bodies[idBody];
        Sprite patternBody = GetBodyPatternById(idBody, idPattern);
        body.GetComponent<SpriteRenderer>().sprite = patternBody;
        return body;
    }

    public FishHead GetHeadById(int idHead, int idPattern) {
        FishHead head = heads[idHead];
        int nPatterns = patternsHeads.Length / heads.Length;
        Sprite patternHead = GetHeadPatternById(idHead, idPattern);
        head.GetComponent<SpriteRenderer>().sprite = patternHead;
        return head;
    }

    public Sprite GetBodyPatternById(int idBody, int idPattern) {
        return patternsBodies[idPattern + idBody * CountPatterns()];
    }

    public Sprite GetHeadPatternById(int idHead, int idPattern) {
        return patternsHeads[idPattern + idHead * CountPatterns()];
    }

    public Fish CreateFish(FishBody body, FishHead head, Transform parent) {

        Fish f = Instantiate(fishPrefab, parent);

        if (body) {
            f.SetBody(body);
            f.transform.localPosition = body.positionSpawn;
            f.transform.localScale = new Vector3(body.scale, body.scale, body.scale);
            // Adjust position for body translation
            f.transform.GetChild(0).localPosition = new Vector3(0, 0, -body.offset.x);
        }
        if (head) {
            f.SetHead(head);
        }

        return f;
    }

    public Fish CreateRandomFish(Transform parent, bool mini = false)
    {
        Fish f = Instantiate(fishPrefab, parent);

        FishBody[] b = mini ? bodiesMini : bodies;
        float[] bw = mini ? bodiesMiniWeights : bodiesWeights;
        FishHead[] h = mini ? headsMini : heads;
        Sprite[] pb = mini ? patternsBodiesMini : patternsBodies;
        Sprite[] ph = mini ? patternsHeadsMini : patternsHeads;

        int nBodies = b.Length;
        int nHeads = h.Length;
        int nPatterns = pb.Length / nBodies;

        //List<string> bodyKeys = new List<string>(fishBodies.Keys);
        //List<string> headKeys = new List<string>(fishHeads.Keys);
        //string bodyStr = bodyKeys[Random.Range(0, bodyKeys.Count)];
        //string headStr = headKeys[Random.Range(0, headKeys.Count)];
        //f.SetBody(bodyStr);
        //f.SetHead(headStr);

        //Decide body
        int bodyInt = 0;
        float rBody = Random.value;
        float wAcc = 0;
        for (int i = 0; i < bw.Length; i++)
        {
            wAcc += bw[i];
            if(rBody < wAcc)
            {
                bodyInt = i;
                break;
            }
        }
        //int bodyInt = rBody < 0.5f ? 0 : (rBody < 0.8f ? 1 : 2);
        //int bodyInt = Random.Range(0, nBodies);

        int headInt = Random.Range(0, nHeads);
        int patternInt = Random.Range(0, nPatterns);

        FishBody body = f.SetBody(b[bodyInt]);
        FishHead head = f.SetHead(h[headInt]);

        //Set patterns
        body.idPattern = (PatternID)patternInt;
        Sprite patternBody = pb[patternInt + bodyInt * nPatterns];
        body.GetComponent<SpriteRenderer>().sprite = patternBody;

        Sprite patternHead = ph[patternInt + headInt * nPatterns];
        head.GetComponent<SpriteRenderer>().sprite = patternHead;

        f.transform.localPosition = body.positionSpawn;
        f.transform.localScale = new Vector3(body.scale, body.scale, body.scale);

        // Adjust position for body translation
        f.transform.GetChild(0).localPosition = new Vector3(0, 0, -body.offset.x);

        return f;
    }

}
