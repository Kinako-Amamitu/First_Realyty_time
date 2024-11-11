<?php

namespace App\Http\Controllers;

use App\Models\Ranking;

class RankingController extends Controller
{
    public function index()
    {
        //DBのすべての要素入手
        $ranking = Ranking::all();
    }
}
