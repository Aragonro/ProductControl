package com.example.alex1.emulcur1;

import android.content.Context;
import android.content.Intent;
import android.os.Build;
import android.preference.PreferenceActivity;
import android.support.annotation.RequiresApi;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.loopj.android.http.*;

import java.net.*;
import java.io.*;
import java.nio.charset.StandardCharsets;

import org.json.*;
import cz.msebera.android.httpclient.*;
import cz.msebera.android.httpclient.client.HttpClient;
import cz.msebera.android.httpclient.client.methods.HttpPost;
import cz.msebera.android.httpclient.entity.StringEntity;
import cz.msebera.android.httpclient.impl.client.HttpClientBuilder;

import org.json.JSONObject;
import android.app.DownloadManager;
import android.content.Intent;
import android.graphics.Color;
import android.os.Build;
import android.preference.PreferenceActivity;
import android.support.annotation.RequiresApi;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;
//import com.android.volley.Response;
//import com.android.volley.Request;
import com.loopj.android.http.AsyncHttpClient;
import com.loopj.android.http.AsyncHttpResponseHandler;
import com.loopj.android.http.RequestParams;

import org.json.JSONObject;

import java.nio.charset.StandardCharsets;

import cz.msebera.android.httpclient.Header;

public class MainActivity extends AppCompatActivity {

    public final static String EMAIL= "EMAIL";
    public final static String TOKEN= "TOKEN";
    public final static String ID_EMULATION= "ID_EMULATION";
    final String BASE_URL = "http://192.168.43.228:8000/";
    private static AsyncHttpClient client = new AsyncHttpClient();
    public int k=1;
    EditText login, password;
    TextView errorText;
    RequestParams getParams() {
        RequestParams params = new RequestParams();
        params.put("Email", login.getText().toString());
        params.put("Password", password.getText().toString());
        return params;
    }
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        login = (EditText)findViewById(R.id.login);
        password = (EditText)findViewById(R.id.password);
        errorText = (TextView)findViewById(R.id.textView);

    }

    private void requestUpdate(final String url) {


                client.get(url, new AsyncHttpResponseHandler() {
                    @Override
                    public void onSuccess(int statusCode, Header[] headers,
                                          byte[] responseBody) {
                        k=1;
                    }

                    @Override
                    public void onFailure(int statusCode, Header[] headers,byte[] responseBody,
                                          Throwable error) {
                        k=0;
                    }
                });


    }
    public void signIn(View view){
        JSONObject jparams = new JSONObject();
        try {
            jparams.put("Email", login.getText().toString());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        try {
            jparams.put("Password", password.getText().toString());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        StringEntity entity = null;
        try {
            entity = new StringEntity(jparams.toString());
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
        Context context = getApplicationContext();
        client.setConnectTimeout(100000);
        client.setResponseTimeout(100000);
        final com.example.alex1.emulcur1.MainActivity da = this;
        client.post(context, "http://192.168.0.73:3000/Account/Login", entity, "application/json", new AsyncHttpResponseHandler() {
            @Override
            public void onSuccess(int statusCode, Header[] headers, byte[] responseBody) {
                String response = new String(responseBody);
                String check = response.substring(1,response.length()-1);
                if(check.equals("Wrong email or password") || check.equals("Bad with user")){
                    errorText.setText(check);
                }
                else{
                    String result = response.substring(18, response.length()-2);
                    listOrders(login.getText().toString(),result);
                }
                /*if (result.length() <= 4){
                    errorText.setText("Wrong email or password");
                }
                else{
                    Intent intent = new Intent(MainActivity.this, UpdateEmulationActivity.class);
                    intent.putExtra("Email", result);
                    startActivity(intent);
                }*/
            }

            @Override
            public void onFailure(int statusCode, Header[] headers, byte[] responseBody, Throwable error) {
                errorText.setText("Server error");
                listOrders("roket98@gmail.com","sdf-asdf-2132");
            }
        });
    }
    public void listOrders(String email, String token){
        Intent intent = new Intent(this, Orders.class);
        intent.putExtra(EMAIL, email);
        intent.putExtra(TOKEN, token);
        // запуск activity
        startActivity(intent);
    }
    /*public void sendUpdate(View view) {



        TextView textview = (TextView) findViewById(R.id.text);
        EditText editText = (EditText) findViewById(R.id.login);
        String message = editText.getText().toString();
        EditText editText_user = (EditText) findViewById(R.id.id_user);
        String message_user = editText_user.getText().toString();
        EditText editText_emulation = (EditText) findViewById(R.id.id_emulation);
        String message_emulation = editText_emulation.getText().toString();

        final String url = "http://192.168.0.73:3000/api/EmulationKits?login=" + message + "&idUser=" + message_user + "&idEmulation=" + message_emulation;

       requestUpdate(url);



        if (k == 1) {
            Intent intent = new Intent(this, UpdateEmulationActivity.class);
            intent.putExtra(LOGIN, message);
            intent.putExtra(ID_USER, message_user);
            intent.putExtra(ID_EMULATION, message_emulation);
            // запуск activity
            startActivity(intent);
        } else {
            textview.setText("Wrong data or server don't work");
        }

        try {
            URL reqURL = new URL("192.168.0.96:3000/api/EmulationKits?login="+message+"&idUser="+message_user+"&idEmulation="+message_emulation); //the URL we will send the request to
            HttpURLConnection request = (HttpURLConnection) (reqURL.openConnection());
            request.setDoOutput(true);
            request.setConnectTimeout(15000);
            request.setRequestMethod("GET");
            request.connect();
            if(request.getResponseCode()>=200 && request.getResponseCode()<300) {


                Intent intent = new Intent(this, UpdateEmulationActivity.class);
                intent.putExtra(LOGIN, message);
                intent.putExtra(ID_USER, message_user);
                intent.putExtra(ID_EMULATION, message_emulation);
                // запуск activity
                startActivity(intent);
            }
            else{
                textview.setText("Wrong data or server don't work");
            }
        }
        catch(Exception e){

            textview.setText("1Wrong data or server don't work");
        }
    }*/

    public void Like(View view) {

    }

    public void Dislike(View view) {

    }
}
