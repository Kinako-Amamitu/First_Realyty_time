<?php

namespace App\Http\Controllers;

use App\Models\Item;
use App\Models\Mail;
use App\Models\Master;
use App\Models\User;
use App\Models\User_mail;
use DebugBar\DebugBar;
use Illuminate\Http\Request;
use Termwind\Components\Dd;

class MailController extends Controller
{
    //マスターデータの取得
    public function index(Request $request)
    {

        //モデル取得
        $mails = Master::join('items', 'masters.item_id', '=', 'items.id')
            ->select('Text', 'item_name', 'masters.id')->get();

        return view('mails.master_data', ['mails' => $mails]);
    }

    //ユーザーメールデータの取得
    public function user(Request $request)
    {

        $users = User::find($request->id);
        if (!empty($user)) {
            $mails = $user->mails()->paginate(2);
            $mails->appends(["id" => $request->id]);
        }
        return view('mails.user_mail', ['users' => $users ?? null]);

    }

    //メール送信画面
    public function send(Request $request)
    {

        return view('mails.mail_send');
    }

    //メール送信処理
    public function resister(Request $request)
    {
        //バリデーション
        $validated = $request->validate([
            'user_id' => ['required'],
            'mail_id' => ['required']
        ]);

        User_mail::create([
            'user_id' => $request['user_id'],
            'mail_id' => $request['mail_id'],
            'item_get' => 0
        ]);
        return redirect()->route('mails.index', ['user_id' => $request['user_id'], 'mail_id' => $request['mail_id']]);
    }
}
