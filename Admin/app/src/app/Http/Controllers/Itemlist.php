<?php

namespace App\Http\Controllers;

use App\Models\Item;
use Illuminate\Http\Request;

class Itemlist extends Controller
{
    //アイテム一覧表示
    public function item(Request $request)
    {
        //ログインセッションチェック
        if (!$request->session()->exists('login')) {
            //ログインにリダイレクト
            return redirect('/');
        }
        //条件を指定して入手
        $items = Item::all();
        return view('items.itemlist', ['accounts' => $items]);
    }

}
