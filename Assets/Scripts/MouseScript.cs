using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{

	public Texture2D cursorTexture;


	// Start is called before the first frame update
	void Start()
    {
		Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width/3, cursorTexture.height/3), CursorMode.Auto);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	
	
	//MyCursor()��� �̸��� �ڷ�ƾ�� ���۵˴ϴ�.
	
}
