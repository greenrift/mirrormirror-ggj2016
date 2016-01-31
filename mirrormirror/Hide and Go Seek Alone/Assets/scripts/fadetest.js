#pragma strict
  
  var duration : float = 1.0;
  var alpha : float = 0;
  
  function Update(){
  
      lerpAlpha();
  }
  
  function lerpAlpha () {
  
  var lerp : float = Mathf.PingPong (Time.time, duration) / duration;
  
      alpha = Mathf.Lerp(0.0, 1.0, lerp) ;
      GetComponent.<Renderer>().material.color.a = alpha;
  }