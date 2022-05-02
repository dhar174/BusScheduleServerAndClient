from ast import While
import json
from flask import Flask, redirect, request
from flask import render_template
from datetime import datetime
from requests.adapters import HTTPAdapter, Retry
import requests
import logging
import time
# from requests.adapters import HTTPAdapter, Retry


app = Flask(__name__)


@app.route('/result')
def countdown(request_string):
    logging.basicConfig(level=logging.DEBUG)

    s = requests.Session()
    retries = Retry(total=20, backoff_factor=1)
    s.mount('https://', HTTPAdapter(max_retries=retries))

    while True:
        try:
            r = s.get(
                request_string, verify=False,  headers={
                    "User-Agent": "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36"
                })
            break
        except Exception as e:
            print(e)
            time.sleep(5)
            continue

    r = s.get(
        request_string, verify=False,  headers={
            "User-Agent": "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36"
        })
    if(r.status_code == 200):
        print("success")
        print(r.text)
        return r.text
    else:
        return "failed"


@app.route('/', endpoint='index')
def index():
    return render_template("index.html")


@app.route('/', methods=['POST'], endpoint='stop_post')
def stop_post():
    by_stop = ''
    try:
        by_stop = request.form['stop']
    except Exception as e:
        print(e)

    if by_stop != '':
        request_string = 'https://localhost:8080/Test/GetNextArrivalTime/{type}/{id}'.format(
            type='stop', id=int(by_stop))
        result = countdown(request_string)
        return render_template(
            "countdown.html",
            # time=numberOfMinutes,
            text=result
        )
    by_route = ''
    try:
        by_route = request.form['route']
    except Exception as e:
        print(e)

    if by_route != '':
        request_string = 'https://localhost:8080/Test/GetNextArrivalTime/{type}/{id}'.format(
            type='route', id=int(by_route))
        result = countdown(request_string)
        return render_template(
            "countdown.html",
            # time=numberOfMinutes,
            text=result
        )
    else:
        return redirect("index.html")


if __name__ == "__app__":
    app.run(ssl_context='adhoc')
