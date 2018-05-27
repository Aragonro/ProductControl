package com.example.alex1.emulcur1;

import android.content.Context;
import android.content.Intent;
import android.media.session.MediaSession;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.TextView;

import com.loopj.android.http.AsyncHttpClient;
import com.loopj.android.http.AsyncHttpResponseHandler;

import org.json.*;

import cz.msebera.android.httpclient.Header;

public class Orders extends AppCompatActivity {

    private String EMAIL;
    private String TOKEN;
    private String BASEURL = "http://192.168.0.73:3000/Order/GetAllOrdersForObserver";
    private static AsyncHttpClient client = new AsyncHttpClient();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Intent intent = getIntent();
        EMAIL = intent.getStringExtra(MainActivity.EMAIL);
        TOKEN = intent.getStringExtra(MainActivity.TOKEN);

        //Context context = getApplicationContext();
        client.setConnectTimeout(100000);
        client.setResponseTimeout(100000);
        String url = BASEURL + "?Email="+EMAIL+"&SecurityStamp="+TOKEN;
        client.post(url, new AsyncHttpResponseHandler() {
            @Override
            public void onSuccess(int statusCode, Header[] headers, byte[] responseBody) {
                String response = new String(responseBody);
                try{
                    JSONArray jsonArray = new JSONArray(response);
                    setOrders(jsonArray);
                }
                catch(Exception ex) {

                }
            }

            @Override
            public void onFailure(int statusCode, Header[] headers, byte[] responseBody, Throwable error) {
                setError();
            }
        });
        /*for(int i=0;i<40;++i){
            TextView textView1 = new TextView(this);
                textView1.setText("Token:"+ TOKEN);
            textView1.setLayoutParams(new ViewGroup.LayoutParams
                    (ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT));
            textView1.setTextSize(26);
            //textView1.setId(i);
            linearLayout.addView(textView1);
        }
        scrollView.addView(linearLayout);
        setContentView(scrollView);*/
    }
    public void setOrders(JSONArray jsonArray){
        ScrollView scrollView = new ScrollView(this);
        LinearLayout linearLayout = new LinearLayout(this);
        linearLayout.setLayoutParams(new ViewGroup.LayoutParams
                (ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT));
        linearLayout.setOrientation(LinearLayout.VERTICAL);
        try{
            for(int i=0;i<jsonArray.length();++i){
                JSONObject jsonObject = jsonArray.getJSONObject(i);
                TextView textView1 = new TextView(this);
                textView1.setText("Delivery Address:"+jsonObject.getString("DeliveryAddress"));
                textView1.setLayoutParams(new ViewGroup.LayoutParams
                        (ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT));
                textView1.setTextSize(18);
                //textView1.setId(i);
                linearLayout.addView(textView1);

                TextView textView2 = new TextView(this);
                textView2.setText("Delivery Date:"+jsonObject.getString("DeliveryDate"));
                textView2.setLayoutParams(new ViewGroup.LayoutParams
                        (ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT));
                textView2.setTextSize(18);
                //textView1.setId(i);
                linearLayout.addView(textView2);

                TextView textView3 = new TextView(this);
                textView3.setText("Price:"+jsonObject.getString("Price"));
                textView3.setLayoutParams(new ViewGroup.LayoutParams
                        (ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT));
                textView3.setTextSize(18);
                //textView1.setId(i);
                linearLayout.addView(textView3);

                TextView textView4 = new TextView(this);
                textView4.setText(" ");
                textView4.setLayoutParams(new ViewGroup.LayoutParams
                        (ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT));
                textView4.setTextSize(40);
                //textView1.setId(i);
                linearLayout.addView(textView4);
            }

        }
        catch(Exception ex){
            TextView textView2 = new TextView(this);
            textView2.setText("Error:"+ex.getMessage());
            textView2.setLayoutParams(new ViewGroup.LayoutParams
                    (ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT));
            textView2.setTextSize(18);
            //textView1.setId(i);
            linearLayout.addView(textView2);
        }
        scrollView.addView(linearLayout);
        setContentView(scrollView);
    }
    public void setError(){
        ScrollView scrollView = new ScrollView(this);
        LinearLayout linearLayout = new LinearLayout(this);
        linearLayout.setLayoutParams(new ViewGroup.LayoutParams
                (ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT));
        linearLayout.setOrientation(LinearLayout.VERTICAL);
        TextView textView1 = new TextView(this);
        textView1.setText("Server Error");
        textView1.setLayoutParams(new ViewGroup.LayoutParams
                (ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT));
        textView1.setTextSize(26);
        //textView1.setId(i);
        linearLayout.addView(textView1);
        scrollView.addView(linearLayout);
        setContentView(scrollView);
    }
}
