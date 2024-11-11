@extends('layouts.app')
@section('title','プレイヤー一覧')
@section('h1','▼プレイヤー一覧▼')
@section('body')
    {{$accounts->links('vendor.pagination.bootstrap-5')}}
    <table class="table table-bordered mx-auto p-2" style="width: 60%">

        <tr>
            <th>ID</th>
            <th>名前</th>
            <th>レベル</th>
            <th>経験値</th>
            <th>ライフ</th>
        </tr>


        @foreach ($accounts as $playerdata)
            <tr>
                <td>{{$playerdata['id']}}</td>
                <td>{{$playerdata['player_name']}}</td>
                <td>{{$playerdata['level']}}</td>
                <td>{{$playerdata['exp']}}</td>
                <td>{{$playerdata['hp']}}</td>
            </tr>
        @endforeach
    </table>
@endsection
