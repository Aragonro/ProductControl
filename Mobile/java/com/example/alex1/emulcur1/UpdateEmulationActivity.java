package com.example.alex1.emulcur1;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;


import com.loopj.android.http.*;

import org.json.JSONObject;

import java.io.OutputStreamWriter;
import java.util.HashMap;
import java.io.BufferedOutputStream;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.io.BufferedWriter;
import cz.msebera.android.httpclient.*;
public class UpdateEmulationActivity extends AppCompatActivity {

    public final static String LOGIN= "LOGIN";
    public final static String ID_USER= "ID_USER";
    public final static String ID_EMULATION= "ID_EMULATION";
    private static AsyncHttpClient client = new AsyncHttpClient();
    public  int k=0;
    String message;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Intent intent = getIntent();
        message = intent.getStringExtra(MainActivity.ID_EMULATION);
        setContentView(R.layout.activity_update_emulation);
    }

    private void requestUpdate(final RequestParams params, final String url) {
try {
    client.post(url, new AsyncHttpResponseHandler() {
        @Override
        public void onSuccess(int statusCode, Header[] headers,
                              byte[] responseBody) {
            k = 1;
        }

        @Override
        public void onFailure(int statusCode, Header[] headers, byte[] responseBody,
                              Throwable error) {
            k = statusCode;
        }
    });
}
catch (Exception ex){
    k=0;
}

    }

    public void sendUpdate(View view) {
        TextView textview = (TextView) findViewById(R.id.text);
        EditText editText_temperature = (EditText) findViewById(R.id.temperature);
        String message_temperature = editText_temperature.getText().toString();
        if(message_temperature.trim().isEmpty()){
            message_temperature="-1000";
        }
        EditText editText_pressure = (EditText) findViewById(R.id.pressure);
        String message_pressure = editText_pressure.getText().toString();
        if(message_pressure.trim().isEmpty()){
            message_pressure="-1000";
        }
        EditText editText_humidity = (EditText) findViewById(R.id.humidity);
        String message_humidity = editText_humidity.getText().toString();
        if(message_humidity.trim().isEmpty()){
            message_humidity="-1000";
        }
        OutputStream out = null;
        RequestParams params = new RequestParams();
        params.put("EmulationKitUpdateId",40);
        params.put("EmulationKitId",message);
        params.put("TemperatureUpdate",message_temperature);
        params.put("PressureUpdate",message_pressure);
        params.put("HumidityUpdate",message_humidity);

        final String data="emulationKitId="+message+"&temperature="+message_temperature+"&pressure="+message_pressure+"&humidity="+message_humidity;
        final String url="http://192.168.0.96:3000/api/EmulationKitUpdates?"+data;
        requestUpdate(params,url);

        if(k==1){
            textview.setText("Successful add update emulation");
        }
        else{
            String s= new Integer(k).toString();
            textview.setText(s);
        }

        /*try {
            URL reqURL = new URL("192.168.0.96:3000/api/EmulationKitUpdates"); //the URL we will send the request to
            HttpURLConnection request = (HttpURLConnection) (reqURL.openConnection());
            HashMap<String, String> postDataParams=new HashMap<>();
            postDataParams.put("EmulationId",message);
            postDataParams.put("TemperatureUpdate",message_temperature);
            postDataParams.put("PressureUpdate",message_pressure);
            postDataParams.put("HumidityUpdate",message_humidity);

            request.setDoOutput(true);
            request.setDoInput(true);
            request.setConnectTimeout(15000);
            request.addRequestProperty("Content-Type", "application/json"); //add the content type of the request, most post data is of this type
            request.setRequestMethod("POST");
            out = new BufferedOutputStream(request.getOutputStream());

            BufferedWriter writer = new BufferedWriter (new OutputStreamWriter(out, "UTF-8"));

            writer.write(postDataParams.toString());

            writer.flush();

            writer.close();

            out.close();

            request.connect();
            out = new BufferedOutputStream(request.getOutputStream());
            if(request.getResponseCode()>=200 && request.getResponseCode()<300) {


                textview.setText("Successful add update emulation");
            }
            else{
                textview.setText("Wrong data or server don't work");
            }
        }
        catch(Exception e){

            textview.setText("1Wrong data or server don't work");
        }*/
    }

    public void Back(View view) {
        /*Intent intent = new Intent(this, MainActivity.class);

        String message = intent.getStringExtra(MainActivity.LOGIN);
        intent.putExtra(LOGIN, message);
        message = intent.getStringExtra(MainActivity.ID_USER);
        intent.putExtra(ID_USER, message);
        message = intent.getStringExtra(MainActivity.ID_EMULATION);
        intent.putExtra(ID_EMULATION, message);
        // запуск activity
        startActivity(intent);*/
    }
}
